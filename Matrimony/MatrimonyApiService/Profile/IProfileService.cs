namespace MatrimonyApiService.Profile;

public interface IProfileService
{
    /// <summary>
    /// Gets profile by given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProfileDto> GetProfileById(int id);
    
    /// <summary>
    /// Gets profile Preview by given id
    /// To be used for profile search
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProfilePreviewDto> GetProfilePreviewById(int id);
    
    /// <summary>
    /// Creates new profile, to be done by the User
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ProfileDto> AddProfile(ProfileDto dto);
    
    // /// <summary>
    // ///  For a user who opts to view a profile,
    // ///  this event will be logged as a profileView.
    // ///  Requires ProfileViewService for operation.
    // /// </summary>
    // /// <param name="viewerId"></param>
    // /// <param name="profileId"></param>
    // /// <returns>Fetched Profile information</returns>
    // Task<ProfileDto> ViewProfile(int viewerId, int profileId);
    
    /// <summary>
    /// Updates Profile
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ProfileDto> UpdateProfile(ProfileDto dto);
    
    /// <summary>
    /// Deletes profile by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProfileDto> DeleteProfileById(int id);
    
    /// <summary>
    ///  Get Matches for the profile, with preference
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns></returns>
    Task<List<ProfilePreviewDto>> GetMatches(int profileId);
    
}