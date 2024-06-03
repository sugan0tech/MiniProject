using System.Security.Cryptography;
using System.Text;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.EntityFrameworkCore;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace MatrimonyApiService.Auth;

public class AuthService(
    IUserService userService,
    ITokenService tokenService,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthReturnDto> Login(LoginDTO loginDto)
    {
        try
        {
            var user = await userService.GetByEmail(loginDto.Email);
            if (!user.IsVerified)
            {
                logger.LogError($"User {user.Email} not verified");
                throw new UserNotVerifiedException($"User {user.Email} not verified");
            }

            var hMACSHA = new HMACSHA512(user.HashKey);
            var hash = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            var isPasswordSame = ComparePassword(hash, user.Password);
            if (isPasswordSame)
            {
                logger.LogInformation($"Successfully logged as Id :{user.UserId}");
                return new AuthReturnDto { Token = tokenService.GenerateToken(user) };
            }

            throw new Exceptions.AuthenticationException("Invalid username or password");
        }
        catch (UserNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    private bool ComparePassword(byte[] encryptedPassword, byte[] password)
    {
        if (encryptedPassword.Length != password.Length)
            return false;

        for (var i = 0; i < encryptedPassword.Length; i++)
            if (encryptedPassword[i] != password[i])
                return false;

        return true;
    }

    public async Task<bool> Register(RegisterDTO dto)
    {
        try
        {
            var hasher = new HMACSHA512();
            var user = new UserDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                AddressId = dto.AddressId,
                Password = hasher.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                HashKey = hasher.Key,
                IsVerified = false,
                LoginAttempts = 0
            };
            await userService.Add(user);
            return true;
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e.Message);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        throw new Exceptions.AuthenticationException("Not able to register at this moment");
    }

    public async Task<AuthReturnDto> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var user = await userService.GetByEmail(resetPasswordDto.Email);

            var hasher = new HMACSHA512(user.HashKey);

            if (!ComparePassword(hasher.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.Password)),
                    user.Password))
                throw new AuthenticationException("Invalid Password");
            user.Password = hasher.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.NewPassword));
            await userService.Update(user);
            return new AuthReturnDto { Token = tokenService.GenerateToken(user) };
        }
        catch (UserNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new Exceptions.AuthenticationException("Failed to reset password");
        }
    }
}