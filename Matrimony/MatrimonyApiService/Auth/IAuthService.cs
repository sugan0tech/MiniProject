using MatrimonyApiService.User;

namespace MatrimonyApiService.Auth;

public interface IAuthService
{
    /// <summary>
    ///  Login returns tokens
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    Task<AuthReturnDto> Login(LoginDTO loginDto);

    /// <summary>
    ///  Logs out user ( invalidate that session )
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task Logout(string token);

    /// <summary>
    ///  gets access token for the valid refresh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<AuthReturnDto> GetAccessToken(string refreshToken);

    /// <summary>
    ///  User account registration.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<UserDto> Register(RegisterDTO dto);
    
    /// <summary>
    ///  User account registration.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="otp"></param>
    /// <returns></returns>
    Task<bool> VerifyUserByOtp(int userId, string otp);

    /// <summary>
    ///  User password change, also invalidates all the other sessions.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<AuthReturnDto> ResetPassword(ResetPasswordDto dto);
}