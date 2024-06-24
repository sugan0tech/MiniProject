namespace MatrimonyApiService.MatchRequest;

public interface IMatchRequestService
{
    /// <summary>
    ///  Returns All the matches Accepted matches for the profile.
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns> List of MatchRequestDTO's </returns>
    Task<List<MatchRequestDto>> GetAcceptedMatchRequests(int profileId);

    /// <summary>
    ///  Returns All the matches Requests.
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns> List of MatchRequestDTO's </returns>
    Task<List<MatchRequestDto>> GetMatchRequests(int profileId);

    /// <summary>
    ///  Returns All the matches Requests.
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns> List of MatchRequestDTO's </returns>
    Task<List<MatchRequestDto>> GetSentMatchRequests(int profileId);

    /// <summary>
    ///  Rejects requested match.
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="profileId"></param>
    Task Reject(int matchId, int profileId);

    /// <summary>
    ///  Approves requested match.
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="profileId"></param>
    Task Approve(int matchId, int profileId);
    
    /// <summary>
    /// Get's match by Id to be  used in other services
    /// </summary>
    /// <param name="id">MatchRequest Id</param>
    /// <returns></returns>
    Task<MatchRequestDto> GetById(int id);

    /// <summary>
    ///  Updated MatchRequest, mostly to be used for updating status ( two way like ).
    /// </summary>
    /// <param name="dto">Update matchDto</param>
    /// <returns>update dto</returns>
    Task<MatchRequestDto> MatchRequestToProfile(int senderId, int targetId);
    
    /// <summary>
    /// Deletes match by id, mostly of if one of the parties dislikes
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MatchRequestDto> DeleteById(int id);

    /// <summary>
    ///  Get's all the matces
    ///  To be only used by admins
    /// </summary>
    /// <returns></returns>
    Task<List<MatchRequestDto>> GetAll();
}