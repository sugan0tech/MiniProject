using System.Security.Cryptography;
using System.Text;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using MatrimonyApiService.UserSession;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Auth;

public class AuthService(
    IUserService userService,
    ITokenService tokenService,
    IUserSessionService userSessionService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthService> logger) : IAuthService
{
    /// <intheritdoc/>
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
                // if staySigned => long lived token
                var tokens = tokenService.GenerateTokens(user, !loginDto.staySigned);
                var newSession = new UserSessionDto
                {
                    UserId = user.UserId,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddMonths(6),
                    DeviceType = GetDeviceType(GetUserAgent()),
                    IpAddress = GetClientIpAddress(),
                    UserAgent = GetUserAgent(),
                    IsValid = true,
                    RefreshToken = tokens.RefreshToken
                };
                await userSessionService.Add(newSession);
                return tokens;
            }

            throw new AuthenticationException("Invalid username or password");
        }
        catch (UserNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task Logout(string accessToken)
    {
        await userSessionService.Invalidate(accessToken);
    }

    /// <intheritdoc/>
    public async Task<AuthReturnDto> GetAccessToken(string refreshToken)
    {
        try
        {
            if (!await userSessionService.IsValid(refreshToken))
                throw new AuthenticationException("Invalid Token, login again please");

            var payload = tokenService.GetPayload(refreshToken);
            var user = await userService.GetById(payload.Id);

            return new AuthReturnDto
                { AccessToken = tokenService.GenerateAccessToken(user), RefreshToken = refreshToken };
        }
        catch (UserNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
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

        throw new AuthenticationException("Not able to register at this moment");
    }

    /// <intheritdoc/>
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

            // logs out from all other devices
            await userSessionService.InvalidateAllPerUser(user.UserId);
            var tokens = tokenService.GenerateTokens(user, false);
            var newSession = new UserSessionDto
            {
                UserId = user.UserId,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMonths(6),
                DeviceType = GetDeviceType(GetUserAgent()),
                IpAddress = GetClientIpAddress(),
                UserAgent = GetUserAgent(),
                IsValid = true,
                RefreshToken = tokens.RefreshToken
            };
            await userSessionService.Add(newSession);
            return tokens;
        }
        catch (UserNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new AuthenticationException("Failed to reset password");
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

    #region ClientInfoHandlers

    private string GetClientIpAddress()
    {
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        return string.IsNullOrWhiteSpace(ipAddress) ? "Unknown IP" : ipAddress;
    }

    private string GetUserAgent()
    {
        return httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ??
               throw new InvalidOperationException("User Agent not found");
    }

    private string GetDeviceType(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent)) return "Unknown Device";

        if (userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase))
        {
            return "Mobile";
        }

        if (userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
        {
            return "Tablet";
        }

        return "Desktop";
    }

    #endregion
}