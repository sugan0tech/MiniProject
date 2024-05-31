namespace MatrimonyApiService.Membership;

public interface IMembershipService
{
    /// <summary>
    ///  Get Membership for a profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <returns></returns>
    Task<MembershipDto> GetByProfileId(int profileId);

    /// <summary>
    ///  Get Membership for a user's profile
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<MembershipDto> GetByUserId(int userId);

    /// <summary>
    /// Deletes Members ship by Id. mostly if user / profile deleted.
    /// </summary>
    /// <param name="membershipId"></param>
    /// <returns></returns>
    Task<MembershipDto> DeleteById(int membershipId);

    /// <summary>
    /// Creating new membership for new Profiles.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<MembershipDto> Add(MembershipDto dto);

    /// <summary>
    ///  Updates Membership details, might be new updated user membership and expiry.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<MembershipDto> Update(MembershipDto dto);

    /// <summary>
    ///  validates given membershipId if it's expired.
    /// </summary>
    /// <param name="membershipId"></param>
    /// <returns></returns>
    Task Validate(int membershipId);

    /// <summary>
    ///  validates given membershipDto if it's expired.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Validate(MembershipDto dto);

    /// <summary>
    ///  Validates all the memberships
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task ValidateAll();
}