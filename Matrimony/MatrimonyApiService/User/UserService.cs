using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;

namespace MatrimonyApiService.User;

public class UserService(
    IBaseRepo<User> repo,
    IProfileService profileService,
    IProfileViewService profileViewService,
    IMembershipService membershipService,
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    /// <intheritdoc/>
    public async Task<ProfileDto> ViewProfile(int userId, int profileId)
    {
        try
        {
            // just for validation;
            var user = await repo.GetById(userId);
            var userProfile = await profileService.GetProfileByUserId(user.Id);
            var membership = await membershipService.GetByProfileId(userProfile.ProfileId);
            if (membership.Type.Equals(MemberShip.FreeUser.ToString()))
            {
                logger.LogWarning($"User has not permission to view Profile {user.Email}");
                throw new NonPremiumUserException($"Current {user.FirstName} runs on free tier account");
            }

            if (membership.Type.Equals(MemberShip.BasicUser.ToString()))
                if (user.Views != null && user.Views.Count() > 50)
                {
                    logger.LogWarning($"User view Profile {user.Email} Has reached to maximum");
                    throw new ExhaustedMaximumProfileViewsException(
                        "You cannot view more than 50 profile per month, Cosider upgrading to premium membership");
                }

            var profile = await profileService.GetProfileById(profileId);
            await profileViewService.AddView(userId, profileId);
            return profile;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<List<UserDto>> GetAll()
    {
        var users = await repo.GetAll();
        return users.ConvertAll(input => mapper.Map<UserDto>(input)).ToList();
    }

    /// <intheritdoc/>
    public async Task<UserDto> Add(User user)
    {
        var usr = await repo.Add(user);
        return mapper.Map<UserDto>(usr);
    }

    /// <intheritdoc/>
    public async Task<UserDto> Update(User user)
    {
        try
        {
            var usr = await repo.Update(user);
            return mapper.Map<UserDto>(user);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<User> GetByEmail(string email)
    {
        var users = await repo.GetAll();
        var user = users.Find(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user == null) throw new UserNotFoundException(email);
        return user;
    }

    public async Task<UserDto> DeleteById(int id)
    {
        try
        {
            return mapper.Map<UserDto>(await repo.DeleteById(id));
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}