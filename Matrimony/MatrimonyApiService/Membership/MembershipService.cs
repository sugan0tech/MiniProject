using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;

namespace MatrimonyApiService.Membership;

public class MembershipService(
    IBaseRepo<Membership> repo,
    IProfileService profileService,
    IMapper mapper,
    ILogger<MembershipService> logger)
    : IMembershipService
{
    /// <inheritdoc/>
    public async Task<MembershipDto> GetByProfileId(int profileId)
    {
        try
        {
            var memberships = await repo.GetAll();
            if (memberships == null)
                throw new KeyNotFoundException("No Memberships Found");
            var membership = memberships.Find(m => m.ProfileId == profileId);
            if (membership == null)
                throw new KeyNotFoundException($"Membership for person with id {profileId} not found.");
            return mapper.Map<MembershipDto>(membership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<MembershipDto> GetByUserId(int userId)
    {
        try
        {
            var profile = await profileService.GetProfileByUserId(userId);
            return await GetByProfileId(profile.ProfileId);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
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
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<MembershipDto> Add(MembershipDto dto)
    {
        try
        {
            await GetByProfileId(dto.ProfileId);
        }
        catch (KeyNotFoundException)
        {
            var membership = mapper.Map<Membership>(dto);
            var addedMembership = await repo.Add(membership);
            return mapper.Map<MembershipDto>(addedMembership);
        }

        throw new AlreadyExistingEntityException($"Membership with profile {dto.ProfileId} already exists");
    }

    /// <inheritdoc/>
    public async Task<MembershipDto> Update(MembershipDto dto)
    {
        try
        {
            var updatedMembership = mapper.Map<Membership>(dto);
            updatedMembership.Id = dto.MembershipId; // Ensure the ID is set to the correct value
            var result = await repo.Update(updatedMembership);
            return mapper.Map<MembershipDto>(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
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
                membership.RequestCount = 0;
                membership.ViewsCount = 0;
                membership.ViewersViewCount = 0;
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
        foreach (var membership in memberships) await Validate(membership.Id);
    }
}