using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;

namespace MatrimonyApiService.Match;

public class MatchService(
    IBaseRepo<Match> repo,
    IProfileService profileService,
    IMapper mapper,
    ILogger<MatchService> logger) : IMatchService
{
    /// <inheritdoc/>
    public async Task<List<MatchDto>> GetAcceptedMatches(int profileId)
    {
        var matches = await repo.GetAll();
        return matches.Where(match => match.SentProfileId.Equals(profileId) && match.ProfileTwoLike).ToList()
            .ConvertAll(input => mapper.Map<MatchDto>(input)).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<MatchDto>> GetMatchRequests(int profileId)
    {
        var matches = await repo.GetAll();
        return matches.Where(match => match.ReceivedProfileId.Equals(profileId)).ToList()
            .ConvertAll(input => mapper.Map<MatchDto>(input)).ToList();
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidMatchForProfile">If the incoming matchId not for current profile</exception>
    public async Task Cancel(int matchId, int profileId)
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
    public async Task<MatchDto> GetById(int id)
    {
        try
        {
            var match = await repo.GetById(id);
            return mapper.Map<MatchDto>(match);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<MatchDto> Add(MatchDto dto)
    {
        var entity = mapper.Map<Match>(dto);
        var savedEntity = await repo.Add(entity);
        return mapper.Map<MatchDto>(savedEntity);
    }

    /// <inheritdoc/>
    public async Task<MatchDto> Update(MatchDto dto)
    {
        try
        {
            var entity = mapper.Map<Match>(dto);
            var updatedEntity = await repo.Update(entity);
            return mapper.Map<MatchDto>(updatedEntity);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<MatchDto> MatchRequestToProfile(int senderId, int targetId)
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

        var match = new MatchDto
        {
            SentProfileId = senderId,
            ReceivedProfileId = targetId,
            FoundAt = DateTime.Now
        };

        return await Add(match);
    }

    /// <inheritdoc/>
    public async Task<MatchDto> DeleteById(int id)
    {
        try
        {
            return mapper.Map<MatchDto>(await repo.DeleteById(id));
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<MatchDto>> GetAll()
    {
        var matches = await repo.GetAll();
        return matches.ConvertAll(input => mapper.Map<MatchDto>(input)).ToList();
    }
}