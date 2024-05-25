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
    ///  Validates if the given membershipId is valid till now.
    /// </summary>
    /// <param name="membershipId"></param>
    /// <returns></returns>
    Task<bool> IsExpired(int membershipId);

    /// <summary>
    ///  Flushes given membershipId if it's expired.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Flush(int membershiptId);

    /// <summary>
    ///  Flushes given membershipDto if it's expired.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Flush(MembershipDto dto);

    /// <summary>
    ///  Flushes all membershipt
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Flush();
}