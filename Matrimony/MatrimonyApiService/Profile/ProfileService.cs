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
        var preference = new PreferenceDto
        {
            MotherTongue = profile.MotherTongue,
            Religion = profile.Religion,
            Education = profile.Education,
            Occupation = profile.Occupation,
            MinHeight = profile.Height - 1,
            MaxHeight = profile.Height + 1,
            MinAge = profile.Age - 5,
            MaxAge = profile.Age + 5
        };
        preference = await preferenceService.Add(preference);
        profile.PreferenceId = preference.PreferenceId;
        return mapper.Map<ProfileDto>(await repo.Add(profile));
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
    public async Task<List<ProfilePreviewDto>> GetMatches(int profileId)
    {
        try
        {
            var profile = await repo.GetById(profileId);
            var preference = await preferenceService.GetById(profile.PreferenceId);
            var profiles = await repo.GetAll();
            var matchedProfiles = profiles.Where(p =>
                (preference.MotherTongue == "ALL" || p.MotherTongue == preference.MotherTongue) &&
                (preference.Religion == "ALL" || p.Religion == preference.Religion) &&
                (preference.Education == "ALL" || p.Education == preference.Education) &&
                (preference.Occupation == "ALL" || p.Occupation == preference.Occupation) &&
                p.Height >= preference.MinHeight && p.Height <= preference.MaxHeight &&
                p.Age >= preference.MinAge && p.Age <= preference.MaxAge
            ).ToList();

            var matchedProfileDtos = matchedProfiles.Select(p => mapper.Map<ProfilePreviewDto>(p)).ToList();

            return matchedProfileDtos;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting matches.");
            throw;
        }
    }
}