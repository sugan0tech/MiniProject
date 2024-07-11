using System.Security.Cryptography;
using System.Text;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Commons.Services;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Membership.Commands;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Preference.Commands;
using MatrimonyApiService.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace MatrimonyApiService.Profile.Commands;

public class CreateProfileCommandHandler(IProfileService profileService,EmailService emailService, IUserService userService, IMediator mediator, MatrimonyContext context, ILogger<CreateProfileCommandHandler> logger)
    : IRequestHandler<CreateProfileCommand, ProfileDto>
{
    public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var userEmail = request.ProfileDto.userEmail;
            if (userEmail != null)
            {
                try
                {
                    var user = await userService.GetByEmail(userEmail);
                    var userProfile = await profileService.GetProfileByUserId(user.UserId);
                    if (userProfile != null)
                    {
                        throw new DuplicateRequestException("This user already have a profile");
                    }
                    request.ProfileDto.UserId = user.UserId;
                }
                // Creating new user and sending login mail
                catch (UserNotFoundException)
                {
                    var hasher = new HMACSHA512();
                    var key = KeyGeneration.GenerateRandomKey(20);
                    var totp = new Totp(key);
                    var passwordPlain = totp.ComputeTotp();
                    var user = new UserDto
                    {
                        Email = userEmail,
                        FirstName = request.ProfileDto.firstName,
                        LastName = request.ProfileDto.lastName,
                        IsVerified = true,
                        Password = hasher.ComputeHash(Encoding.UTF8.GetBytes(passwordPlain)),
                        HashKey = hasher.Key
                    };
                    
                    var newUser = await userService.Add(user);
                    await userService.Validate(newUser.UserId, true);
                    emailService.SendEmail(user.Email, "Account Registration", $"Welcome! Your password is {passwordPlain}. Please change it after logging in.");
                    request.ProfileDto.UserId = newUser.UserId;
                }
            }
            var profile = await profileService.AddProfile(request.ProfileDto);

            // Creating membership
            var membershipDto = new MembershipDto
            {
                ProfileId = profile.ProfileId,
                Type = MemberShip.FreeUser.ToString(),
                EndsAt = DateTime.Now.AddDays(30),
                IsTrail = false,
                IsTrailEnded = false
            };

            var membership = await mediator.Send(new CreateMembershipCommand(membershipDto));

            // Creating Preference
            var preferenceDto = new PreferenceDto
            {
                PreferenceForId = profile.ProfileId,
                Gender = profile.Gender.Equals(Gender.Male.ToString())
                    ? Gender.Female.ToString()
                    : Gender.Male.ToString(),
                MotherTongue = profile.MotherTongue,
                Religion = profile.Religion,
                Education = profile.Education,
                Occupation = profile.Occupation,
                MinHeight = profile.Height - 1,
                MaxHeight = profile.Height + 1,
                MinAge = profile.Age - 5,
                MaxAge = profile.Age + 5,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var preference = await mediator.Send(new CreatePreferenceCommand(preferenceDto));

            profile.MembershipId = membership.MembershipId;
            profile.PreferenceId = preference.PreferenceId;
            await profileService.UpdateProfile(profile);

            await transaction.CommitAsync(cancellationToken);

            return profile;
        }
        catch (DuplicateRequestException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw ;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex.Message);
            throw new DbUpdateException("Transaction failed, rolling back." + ex.Message);
        }
    }
}