using System.Diagnostics;
using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Preference;
using MatrimonyApiService.ProfileView;

namespace MatrimonyApiService.Profile;

public class ProfileService(
    IBaseRepo<Profile> repo,
    IPreferenceService preferenceService,
    IProfileViewService profileViewService,
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

    public async Task<List<ProfilePreviewDto>> GetProfilePreviewForManager(int managerId)
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
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting matches.");
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<List<ProfileViewDto>> GetViews(int profileId)
    {
        try
        {
            var profile = await repo.GetById(profileId);
            var membership = profile.Membership;
            var views = await profileViewService.GetViewsByProfileId(profileId);
            if (membership == null || membership.Type.Equals(MemberShip.FreeUser.ToString()))
                throw new NonPremiumUserException("Atleast you have to be a basic user to access this feature");
            if (membership.Type.Equals(MemberShip.BasicUser.ToString()))
            {
                var filteredViews = views
                    .Where(view => view.ViewedProfileAt == profileId && view.ViewedAt > DateTime.Now.AddMonths(-1))
                    .OrderBy(view => view.ViewedAt)
                    .Take(5)
                    .ToList();
                return filteredViews;
            }

            return views;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError($"Profile with {profileId} not found");
            throw;
        }
    }
}