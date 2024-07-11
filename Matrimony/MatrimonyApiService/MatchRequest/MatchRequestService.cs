using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;

namespace MatrimonyApiService.MatchRequest;

public class MatchRequestService(
    IBaseRepo<MatchRequest> repo,
    IProfileService profileService,
    IMembershipService membershipService,
    IPreferenceService preferenceService,
    IMapper mapper,
    ILogger<MatchRequestService> logger) : IMatchRequestService
{
    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetAcceptedMatchRequests(int profileId)
    {
        await profileService.GetProfileById(profileId); // validates profile
        var matches = await repo.GetAll();
        return matches.Where(match => (match.SentProfileId.Equals(profileId) || match.ReceivedProfileId.Equals(profileId)) && match.ReceiverLike).ToList()
            .ConvertAll(input => mapper.Map<MatchRequestDto>(input)).ToList();
    }
    
    /// <summary>
    ///  Given two profile Id, returns if any match present
    /// </summary>
    /// <param name="profileOneId"></param>
    /// <param name="profileTwoId"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<MatchRequestDto> IsThereAMatch(int profileOneId, int profileTwoId)
    {
        await profileService.GetProfileById(profileOneId); // validates profile
        await profileService.GetProfileById(profileTwoId); // validates profile
        var matches = await repo.GetAll();
        try
        {
            var match = matches.First(match =>
                (match.SentProfileId.Equals(profileOneId) || match.SentProfileId.Equals(profileTwoId)) &&
                match.ReceiverLike &&
                (match.ReceivedProfileId.Equals(profileOneId) || match.ReceivedProfileId.Equals(profileTwoId)));
            return mapper.Map<MatchRequestDto>(match);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            throw new EntityNotFoundException("No match found for these profiles!!");
        }
    }

    /// <inheritdoc/>
    public async Task<List<MatchRequestDto>> GetMatchRequests(int profileId)
    {
        var matches = await repo.GetAll();
        return matches.Where(match => match.ReceivedProfileId.Equals(profileId) && !match.ReceiverLike).ToList()
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
            match.ReceiverLike = false;
            match.IsRejected = true;
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
            match.ReceiverLike = true;
            match.IsRejected = false;
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
        var senderProfile = await profileService.GetProfileById(senderId);
        await profileService.GetProfileById(targetId);

        var membership = await membershipService.GetByProfileId(senderId);
        if (!membership.IsTrail)
        {
            if (membership.Type.Equals(MemberShip.FreeUser.ToString()) && membership.RequestCount >= 5)
                throw new ExhaustedMaximumRequestsException(
                    "You have exhausted out of your monthly quot of 5 requests, Consider upgrading to high tier membership");
            if (membership.Type.Equals(MemberShip.BasicUser.ToString()) && membership.RequestCount >= 15)
                throw new ExhaustedMaximumRequestsException(
                    "You have exhausted out of your monthly quot of 15 requests, Consider upgrading to high tier membership");
        }

        var matches = await GetAll();
        foreach (var matchDto in matches)
        {
            if (matchDto.SentProfileId == senderId && matchDto.ReceivedProfileId == targetId)
                throw new DuplicateRequestException($"You have already sent request for this Profile {targetId}");
        }
        
        // Fetch sender's preferences
        var preference = await preferenceService.GetByProfileId(targetId);
        if (preference == null)
            throw new Exception("Target's preferences not found.");
        
        int level = CalculateMatchLevel(preference, senderProfile);
                    
        var match = new MatchRequestDto
        {
            SentProfileId = senderId,
            ReceivedProfileId = targetId,
            FoundAt = DateTime.Now,
            Level = level
        };

        var entity = mapper.Map<MatchRequest>(match);
        var savedEntity = await repo.Add(entity);
        membership.RequestCount++;
        await membershipService.Update(membership);
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
    private int CalculateMatchLevel(PreferenceDto preference,ProfileDto receiverProfile)
    {
        int level = 0;
    
        if (preference.Gender == receiverProfile.Gender) level++;
        if (preference.MotherTongue == "ALL" || preference.MotherTongue == receiverProfile.MotherTongue) level++;
        if (preference.Religion == "ALL" || preference.Religion == receiverProfile.Religion) level++;
        if (preference.Education == "ALL" || preference.Education == receiverProfile.Education) level++;
        if (preference.Occupation == "ALL" || preference.Occupation == receiverProfile.Occupation) level++;
        if (receiverProfile.Height >= preference.MinHeight && receiverProfile.Height <= preference.MaxHeight) level++;
        if (receiverProfile.Age >= preference.MinAge && receiverProfile.Age <= preference.MaxAge) level++;
    
        return level;
    }
}