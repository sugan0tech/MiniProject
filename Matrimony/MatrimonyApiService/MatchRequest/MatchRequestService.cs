using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;

namespace MatrimonyApiService.MatchRequest;

public class MatchRequestService(
    IBaseRepo<MatchRequest> repo,
    IProfileService profileService,
    IMapper mapper,
    ILogger<MatchRequestService> logger) : IMatchRequestService
{
    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetAcceptedMatcheRequests(int profileId)
    {
        await profileService.GetProfileById(profileId); // validates profile
        var matches = await repo.GetAll();
        return matches.Where(match => match.SentProfileId.Equals(profileId) && match.ProfileTwoLike).ToList()
            .ConvertAll(input => mapper.Map<MatchRequestDto>(input)).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetMatchRequests(int profileId)
    {
        var matches = await repo.GetAll();
        return matches.Where(match => match.ReceivedProfileId.Equals(profileId)).ToList()
            .ConvertAll(input => mapper.Map<MatchRequestDto>(input)).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetSentMatchRequests(int profileId)
    {
        await profileService.GetProfileById(profileId);
        var requests = await GetAll();
        return requests.FindAll(dto => dto.SentProfileId.Equals(profileId));
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidMatchForProfile">If the incoming matchId not for current profile</exception>
    public async Task Reject(int matchId, int profileId)
    {
        var match = await repo.GetById(matchId);
        if (match.ReceivedProfileId.Equals(profileId))
        {
            match.ProfileTwoLike = false;
            await repo.Update(match);
            return;
        }

        logger.LogError($"The match {matchId} is not meant for {profileId}");
        throw new InvalidMatchForProfile($"The match {matchId} is not meant for {profileId}");
    }
    
    /// <inheritdoc/>
    /// <exception cref="InvalidMatchForProfile">If the incoming matchId not for current profile</exception>
    public async Task Approve(int matchId, int profileId)
    {
        var match = await repo.GetById(matchId);
        if (match.ReceivedProfileId.Equals(profileId))
        {
            match.ProfileTwoLike = true;
            await repo.Update(match);
            return;
        }

        logger.LogError($"The match {matchId} is not meant for {profileId}");
        throw new InvalidMatchForProfile($"The match {matchId} is not meant for {profileId}");
    }

    /// <inheritdoc/>
    public async Task<MatchRequestDto> GetById(int id)
    {
        try
        {
            var match = await repo.GetById(id);
            return mapper.Map<MatchRequestDto>(match);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<MatchRequestDto> MatchRequestToProfile(int senderId, int targetId)
    {
        if (senderId == targetId)
            throw new MatchRequestToSelfException($"{senderId} is trying to give self request");

        // validations
        await profileService.GetProfileById(senderId);
        await profileService.GetProfileById(targetId);

        var matches = await GetAll();
        foreach (var matchDto in matches)
        {
            if (matchDto.SentProfileId == senderId && matchDto.ReceivedProfileId == targetId)
                throw new DuplicateRequestException($"You have already sent request for this Profile {targetId}");
        }

        var match = new MatchRequestDto
        {
            SentProfileId = senderId,
            ReceivedProfileId = targetId,
            FoundAt = DateTime.Now
        };

        var entity = mapper.Map<MatchRequest>(match);
        var savedEntity = await repo.Add(entity);
        return mapper.Map<MatchRequestDto>(savedEntity);
    }

    /// <inheritdoc/>
    public async Task<MatchRequestDto> DeleteById(int id)
    {
        try
        {
            return mapper.Map<MatchRequestDto>(await repo.DeleteById(id));
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetAll()
    {
        var matches = await repo.GetAll();
        return matches.ConvertAll(input => mapper.Map<MatchRequestDto>(input)).ToList();
    }
}