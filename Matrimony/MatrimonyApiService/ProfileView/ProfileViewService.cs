using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.ProfileView;

using System;
using System.Threading.Tasks;

public class ProfileViewService(IBaseRepo<ProfileView> profileViewRepo, IMapper mapper) : IProfileViewService
{

    /// <inheritdoc/>
    public async Task AddView(int viewerId, int profileId)
    {
        try
        {
            var profileView = new ProfileView { ViewedProfileAt = profileId, ViewerId = viewerId, ViewedAt = DateTime.Now};
            await profileViewRepo.Add(profileView);
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding profile view.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task AddView(ProfileViewDto profileViewDto)
    {
        try
        {
            var profileView = mapper.Map<ProfileView>(profileViewDto);
            await profileViewRepo.Add(profileView);
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding profile view.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<ProfileViewDto> GetViewById(int viewId)
    {
        try
        {
            var profileViewEntity = await profileViewRepo.GetById(viewId);
            return mapper.Map<ProfileViewDto>(profileViewEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting profile view with id {viewId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task DeleteViewById(int viewId)
    {
        try
        {
            await profileViewRepo.DeleteById(viewId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting profile view with id {viewId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task DeleteOldViews(DateTime before)
    {
        try
        {
            var views = await profileViewRepo.GetAll();
            var viewsToDelete = views.Where(view => view.ViewedAt.CompareTo(before) < 0).ToList();
            foreach (var viewToDelete in viewsToDelete)
            {
                await profileViewRepo.DeleteById(viewToDelete.Id);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting old profile views.", ex);
        }
    }
}