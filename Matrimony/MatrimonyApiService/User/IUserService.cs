using MatrimonyApiService.Profile;

namespace MatrimonyApiService.User;

public interface IUserService
{
    Task<ProfileDto> viewProfile(int userId, int profileId);
}