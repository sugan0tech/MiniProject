using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;

namespace MatrimonyApiService.ProfileView;

using System;
using System.Threading.Tasks;

public class ProfileViewService(
    IBaseRepo<ProfileView> profileViewRepo,
    IMapper mapper,
    ILogger<ProfileViewService> logger) : IProfileViewService
{
    /// <inheritdoc/>
    public async Task AddView(int viewerId, int profileId)
    {
        var profileView = new ProfileView { ViewedProfileAt = profileId, ViewerId = viewerId, ViewedAt = DateTime.Now };
        await profileViewRepo.Add(profileView);
    }

    /// <inheritdoc/>
    public async Task AddView(ProfileViewDto profileViewDto)
    {
        var profileView = mapper.Map<ProfileView>(profileViewDto);
        await profileViewRepo.Add(profileView);
    }

    /// <inheritdoc/>
    public async Task<ProfileViewDto> GetViewById(int viewId)
    {
        try
        {
            var profileViewEntity = await profileViewRepo.GetById(viewId);
            return mapper.Map<ProfileViewDto>(profileViewEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<List<ProfileViewDto>> GetViewsByProfileId(int profileId)
    {
        var views = await profileViewRepo.GetAll();
        return views.FindAll(view => view.ViewedProfileAt.Equals(profileId))
            .ConvertAll(view => mapper.Map<ProfileViewDto>(view)).ToList();
    }

    /// <inheritdoc/>
    public async Task DeleteViewById(int viewId)
    {
        try
        {
            await profileViewRepo.DeleteById(viewId);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteOldViews(DateTime before)
    {
        if (DateTime.Now < before)
        {
            logger.LogError($"{before} is in near feature, Cleanup dates should be atleast a day older");
            throw new InvalidDateTimeException(
                $"{before} is in near feature, Cleanup dates should be atleast a day older");
        }

        var views = await profileViewRepo.GetAll();
        var viewsToDelete = views.Where(view => view.ViewedAt.CompareTo(before) < 0).ToList();
        foreach (var viewToDelete in viewsToDelete) await profileViewRepo.DeleteById(viewToDelete.Id);
    }
}