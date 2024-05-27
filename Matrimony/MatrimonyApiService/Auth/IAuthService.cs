using MatrimonyApiService.User;

namespace MatrimonyApiService.Auth;

public interface IAuthService
{
    Task<AuthReturnDto> Login(LoginDTO loginDto);
    Task<bool> Register(RegisterDTO dto);
    Task<AuthReturnDto> ResetPassword(ResetPasswordDto dto);
}