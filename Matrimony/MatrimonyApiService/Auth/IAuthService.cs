using MatrimonyApiService.User;

namespace MatrimonyApiService.Auth;

public interface IAuthService
{
    Task<UserAuthReturnDto> Login(UserLoginDto loginDto);
    Task<bool> Register(UserRegisterDto dto);
    Task<UserAuthReturnDto> ResetPassword(ResetPasswordDto dto);
}