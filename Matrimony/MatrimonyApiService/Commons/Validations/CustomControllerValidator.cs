using System.Security.Claims;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;

namespace MatrimonyApiService.Commons.Validations;

/// <summary>
/// Service provides validations with user token and api parameter, else throws respective exceptions.
/// </summary>
/// <param name="profileService"></param>
public class CustomControllerValidator(IProfileService profileService)
{
    /// <summary>
    ///  Validates parameter User id with logged user privilege
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="parameterUserId"></param>
    /// <exception cref="AuthenticationException"></exception>
    public void ValidateUserPrivilege(IEnumerable<Claim> claims, int parameterUserId)
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

    /// <summary>
    ///  Validates given profile with logged user
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="profileId"></param>
    /// <exception cref="AuthenticationException"></exception>
    public async Task ValidateUserPrivilegeForProfile(IEnumerable<Claim> claims, int profileId)
    {
        var profile = await profileService.GetProfileById(profileId);
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var usrId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var role = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (role is "RefreshToken")
            throw new AuthenticationException($"Using Refresh Token type is prohibited");
        if (role is "Admin")
            return;
        if (usrId != null)
        {
            var userId = int.Parse(usrId);
            if (profile.UserId.Equals(userId) || profile.ManagedById.Equals(userId))
                return;
        }

        throw new AuthenticationException($"You {email} dont have permission for this action");
    }

    /// <summary>
    ///  Validate User.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="ids"></param>
    /// <exception cref="AuthenticationException"></exception>
    public void ValidateUserPrivilege(IEnumerable<Claim> claims, (int, int) ids)
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