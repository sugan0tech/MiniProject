namespace MatrimonyApiService.Auth;

public interface ITokenService
{
    public string GenerateToken(User.User user);
    public PayloadDto GetPayload(string token);
}