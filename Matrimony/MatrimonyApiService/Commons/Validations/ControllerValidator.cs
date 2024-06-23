using System.Security.Claims;
using MatrimonyApiService.Exceptions;

namespace MatrimonyApiService.Commons.Validations;

public class ControllerValidator
{
    public static void ValidateUserPrivilege(IEnumerable<Claim> claims, int parameterUserId)
    {
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var usrId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var role = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (role is "RefreshToken")
            throw new AuthenticationException($"Using Refresh Token type is prohibitted");
        if (role is "Admin")
            return;
        if (usrId != null && parameterUserId.Equals(int.Parse(usrId)))
            return;

        throw new AuthenticationException($"You {email} dont have permission for this action");
    }

    public static void ValidateUserPrivilege(IEnumerable<Claim> claims, (int, int) ids)
    {
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var usrId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var role = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (role is "Admin")
            return;
        if (usrId != null && new[] { ids.Item1, ids.Item2 }.Contains(int.Parse(usrId)))
            return;

        throw new AuthenticationException($"You {email} dont have permission for this action");
    }
}