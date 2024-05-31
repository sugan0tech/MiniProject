using MatrimonyApiService.User;

namespace MatrimonyApiService.Auth;

public interface ITokenService
{
    public string GenerateToken(UserDto user);
    public PayloadDto GetPayload(string token);
}