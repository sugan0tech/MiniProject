using MatrimonyApiService.Profile;

namespace MatrimonyApiService.User;

public interface IUserService
{
    Task<ProfileDto> ViewProfile(int userId, int profileId);
    Task<List<User>> GetAll();
    // TODO mail validation
    /// <summary>
    ///  To create new user, to be used in Auth service
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<UserDto> Add(User user);
    /// <summary>
    ///  To update user, to be used in Auth service
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<UserDto> Update(User user);
    Task<User> GetByEmail(string email);
}