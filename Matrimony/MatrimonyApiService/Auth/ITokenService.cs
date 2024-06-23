using MatrimonyApiService.User;

namespace MatrimonyApiService.Auth;

public interface ITokenService
{
    /// <summary>
    ///  Generates JWT token with given user & expiration
    /// </summary>
    /// <param name="user"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    public string GenerateToken(UserDto user, DateTime expiration);
    
    /// <summary>
    ///  Generates Access Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateAccessToken(UserDto user);
    
    /// <summary>
    ///  Generates Refersh Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateRefreshToken(UserDto user);
    
    /// <summary>
    /// Generates sets of JWT token ( access & refresh )
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public AuthReturnDto GenerateTokens(UserDto user);
    
    /// <summary>
    /// Extracts and returns payload of a token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public PayloadDto GetPayload(string token);
}