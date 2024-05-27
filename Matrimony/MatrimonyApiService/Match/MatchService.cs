using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;

namespace MatrimonyApiService.Match;

public class MatchService(IBaseRepo<Match> repo, IMapper mapper, ILogger<MatchService> logger) : IMatchService
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
        var entity = mapper.Map<Match>(dto);
        var updatedEntity = await repo.Update(entity);
        return mapper.Map<MatchDto>(updatedEntity);
    }

    /// <inheritdoc/>
    public async Task<MatchDto> DeleteById(int id)
    {
        return mapper.Map<MatchDto>(await repo.DeleteById(id));
    }

    /// <inheritdoc/>
    public async Task<List<MatchDto>> GetAll()
    {
        var matches = await repo.GetAll();
        return matches.ConvertAll(input => mapper.Map<MatchDto>(input)).ToList();
    }
}