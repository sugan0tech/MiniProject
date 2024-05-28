using MatrimonyApiService.Profile;

namespace MatrimonyApiService.User;

public interface IUserService
{
    Task<ProfileDto> ViewProfile(int userId, int profileId);

    /// <summary>
    /// To be used by Admins
    /// </summary>
    /// <returns></returns>
    Task<List<UserDto>> GetAll();

    /// <summary>
    /// To be used by Admins
    /// </summary>
    /// <returns></returns>
    Task<UserDto> DeleteById(int id);

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

    /// <summary>
    /// Get's user by Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<User> GetByEmail(string email);
}