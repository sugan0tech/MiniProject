using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;

namespace MatrimonyApiService.Membership;

public class MembershipService(IBaseRepo<Membership> repo, IMapper mapper, ILogger<Membership> logger)
    : IMembershipService
{
    /// <inheritdoc/>
    public async Task<MembershipDto> GetByPersonId(int personId)
    {
        try
        {
            var memberships = await repo.GetAll();
            var membership = memberships.Find(m => m.ProfileId == personId);
            if (membership == null)
                throw new KeyNotFoundException($"Membership for person with id {personId} not found.");
            return mapper.Map<MembershipDto>(membership);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving membership for person with id {personId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<MembershipDto> DeleteById(int membershipId)
    {
        try
        {
            var deletedMembership = await repo.DeleteById(membershipId);
            return mapper.Map<MembershipDto>(deletedMembership);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting membership with id {membershipId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<MembershipDto> Add(MembershipDto dto)
    {
        try
        {
            var membership = mapper.Map<Membership>(dto);
            var addedMembership = await repo.Add(membership);
            return mapper.Map<MembershipDto>(addedMembership);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding membership.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<MembershipDto> Update(MembershipDto dto)
    {
        try
        {
            var existingMembership = await repo.GetById(dto.MembershipId);
            if (existingMembership == null)
            {
                throw new KeyNotFoundException($"Membership with id {dto.MembershipId} not found.");
            }

            var updatedMembership = mapper.Map<Membership>(dto);
            updatedMembership.Id = dto.MembershipId; // Ensure the ID is set to the correct value
            var result = await repo.Update(updatedMembership);
            return mapper.Map<MembershipDto>(result);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating membership with id {dto.MembershipId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task Validate(int membershipId)
    {
        try
        {
            var membership = await repo.GetById(membershipId);
            if (membership.EndsAt < DateTime.Now)
            {
                if (membership.IsTrail)
                    membership.IsTrailEnded = true;

                membership.TypeEnum = MemberShip.FreeUser;
                logger.LogInformation($"Membership Ended => Profile: {membership.ProfileId}");
            }

            await repo.Update(membership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task Validate(MembershipDto dto)
    {
        await Validate(dto.MembershipId);
    }

    /// <inheritdoc/>
    public async Task ValidateAll()
    {
        var memberships = await repo.GetAll();
        foreach (var membership in memberships)
        {
            await Validate(membership.Id);
        }
    }
}