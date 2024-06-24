namespace MatrimonyApiService.UserSession;

public interface IUserSessionService
{
    /// <summary>
    /// Gets session by Id
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<List<UserSessionDto>> GetById(int sessionId);

    /// <summary>
    /// Gets session of user by userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<UserSessionDto> GetByUserId(int userId);

    /// <summary>
    /// To be used by Admins
    /// </summary>
    /// <returns></returns>
    Task<List<UserSessionDto>> GetAll();

    /// <summary>
    /// To be used by Admins, deletes a session by id
    /// </summary>
    /// <returns></returns>
    Task<UserSessionDto> DeleteById(int id);

    /// <summary>
    /// Deletes all expired sessions.
    /// </summary>
    /// <returns></returns>
    Task Flush(int id);

    /// <summary>
    ///  To create new user session, to be used in Auth service
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<UserSessionDto> Add(UserSessionDto user);

    /// <summary>
    ///  Validates token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> IsValid(string token);

    /// <summary>
    ///  Invalidate given session
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserSessionDto> Invalidate(string token);

    /// <summary>
    ///  Invalidates all the loged in session of a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task InvalidateAllPerUser(int userId);
}