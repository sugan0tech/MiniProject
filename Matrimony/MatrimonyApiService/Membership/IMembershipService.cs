namespace MatrimonyApiService.Membership;

public interface IMembershipService
{
    /// <summary>
    ///  Get Membership for a person
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<MembershipDto> GetByPersonId(int personId);

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
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Validate(int membershiptId);

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