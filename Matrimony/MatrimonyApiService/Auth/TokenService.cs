using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MatrimonyApiService.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace MatrimonyApiService.Auth;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        var secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value;
        if (secretKey == null)
            throw new NoSecretKeyFoundException("No Token generation Secret key found for this Environment");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    public string GenerateToken(User.User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email)
        };
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2),
            signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(myToken);
        return token;
    }

    public PayloadDto GetPayload(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
        tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        var jwtToken = (JwtSecurityToken)validatedToken;
        var claims = jwtToken.Claims;
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var payload = new PayloadDto
        (
            int.Parse(enumerable.First(x => x.Type == ClaimTypes.Name).Value),
            enumerable.First(x => x.Type == ClaimTypes.Email).Value
        );

        return payload;
    }
}