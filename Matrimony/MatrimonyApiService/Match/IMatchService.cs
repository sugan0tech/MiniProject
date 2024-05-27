namespace MatrimonyApiService.Match;

public interface IMatchService
{
    /// <summary>
    ///  Returns All the matches Accepted matches for the profile.
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns> List of MatchDto's </returns>
    Task<List<MatchDto>> GetAcceptedMatches(int profileId);

    /// <summary>
    ///  Returns All the matches Requests.
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns> List of MatchDto's </returns>
    Task<List<MatchDto>> GetMatchRequests(int profileId);
    
    /// <summary>
    ///  Rejects requested match.
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="profileId"></param>
    Task Cancel(int matchId, int profileId);
    
    /// <summary>
    /// Get's match by Id to be  used in other services
    /// </summary>
    /// <param name="id">Match Id</param>
    /// <returns></returns>
    Task<MatchDto> GetById(int id);

    /// <summary>
    /// Saves Match to store, to be used only among services.
    /// </summary>
    /// <param name="dto">Match dto to be stored</param>
    /// <returns></returns>
    Task<MatchDto> Add(MatchDto dto);

    /// <summary>
    ///  Updated Match, mostly to be used for updating status ( two way like ).
    /// </summary>
    /// <param name="dto">Update matchDto</param>
    /// <returns>update dto</returns>
    Task<MatchDto> Update(MatchDto dto);

    /// <summary>
    /// Deletes match by id, mostly of if one of the parties dislikes
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MatchDto> DeleteById(int id);

    /// <summary>
    ///  Get's all the matces
    ///  To be only used by admins
    /// </summary>
    /// <returns></returns>
    Task<List<MatchDto>> GetAll();
}