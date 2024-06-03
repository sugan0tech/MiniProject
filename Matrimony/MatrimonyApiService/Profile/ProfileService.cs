using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Preference;

namespace MatrimonyApiService.Profile;

public class ProfileService(
    IBaseRepo<Profile> repo,
    IPreferenceService preferenceService,
    IMapper mapper,
    ILogger<ProfileService> logger) : IProfileService
{
    /// <intheritdoc/>
    public async Task<ProfileDto> GetProfileById(int id)
    {
        try
        {
            var profile = await repo.GetById(id);
            return mapper.Map<ProfileDto>(profile);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<ProfileDto> GetProfileByUserId(int userId)
    {
        var profiles = await repo.GetAll();
        var fetchedProfile = profiles.Find(profile => profile.UserId.Equals(userId));
        return mapper.Map<ProfileDto>(fetchedProfile);
    }

    public async Task<List<ProfilePreviewDto>> GetProfilesByManager(int managerId)
    {
        var profiles = await repo.GetAll();
        return profiles.FindAll(profiles => profiles.ManagedById.Equals(managerId))
            .ConvertAll(profile => mapper.Map<ProfilePreviewDto>(profile)).ToList();
    }

    /// <intheritdoc/>
    public async Task<ProfilePreviewDto> GetProfilePreviewById(int id)
    {
        try
        {
            var profile = await repo.GetById(id);
            return mapper.Map<ProfilePreviewDto>(profile);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<ProfileDto> AddProfile(ProfileDto dto)
    {
        var profile = mapper.Map<Profile>(dto);
        var savedProfile = await repo.Add(profile);
        var preference = new PreferenceDto
        {
            PreferenceForId = savedProfile.Id,
            MotherTongue = profile.MotherTongue,
            Religion = profile.Religion,
            Education = profile.Education,
            Occupation = profile.Occupation,
            MinHeight = profile.Height - 1,
            MaxHeight = profile.Height + 1,
            MinAge = profile.Age - 5,
            MaxAge = profile.Age + 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        preference = await preferenceService.Add(preference);
        profile.PreferenceId = preference.PreferenceId;
        await repo.Update(savedProfile);
        return mapper.Map<ProfileDto>(savedProfile);
    }

    /// <intheritdoc/>
    public async Task<ProfileDto> UpdateProfile(ProfileDto dto)
    {
        try
        {
            var profile = await repo.Update(mapper.Map<Profile>(dto));
            return mapper.Map<ProfileDto>(profile);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<ProfileDto> DeleteProfileById(int id)
    {
        try
        {
            var profile = await repo.DeleteById(id);
            return mapper.Map<ProfileDto>(profile);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<List<ProfilePreviewDto>> GetProfileMatches(int profileId)
    {
        try
        {
            var profile = await repo.GetById(profileId);
            var preference = await preferenceService.GetById((int)profile.PreferenceId!);
            var profiles = await repo.GetAll();
            var matchedProfiles = profiles.Where(p =>
                (preference.MotherTongue == "ALL" || p.MotherTongue == preference.MotherTongue) &&
                (preference.Religion == "ALL" || p.Religion == preference.Religion) &&
                (preference.Education == "ALL" || p.Education == preference.Education) &&
                (preference.Occupation == "ALL" || p.Occupation == preference.Occupation) &&
                p.Height >= preference.MinHeight && p.Height <= preference.MaxHeight &&
                p.Age >= preference.MinAge && p.Age <= preference.MaxAge &&
                p.Id != profileId
            ).ToList();

            var matchedProfileDtos = matchedProfiles.Select(p => mapper.Map<ProfilePreviewDto>(p)).ToList();

            return matchedProfileDtos;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<List<ProfileDto>> GetAll()
    {
        var profiles = await repo.GetAll();
        return profiles.ConvertAll(profile => mapper.Map<ProfileDto>(profile)).ToList();
    }
}