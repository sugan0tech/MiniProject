using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;

namespace MatrimonyApiService.User;

public class UserService(
    IBaseRepo<User> repo,
    IProfileService profileService,
    IProfileViewService profileViewService,
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    public async Task<ProfileDto> ViewProfile(int userId, int profileId)
    {
        try
        {
            var profile = await profileService.GetProfileById(profileId);
            // just for validation;
            _ = await repo.GetById(userId);
            await profileViewService.AddView(userId, profileId);
            return profile;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public Task<List<User>> GetAll()
    {
        return repo.GetAll();
    }

    public async Task<UserDto> Add(User user)
    {
        var usr = await repo.Add(user);
        return mapper.Map<UserDto>(usr);
    }

    public async Task<UserDto> Update(User user)
    {
        try
        {
            var usr = await repo.Update(user);
            return mapper.Map<UserDto>(user);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<User> GetByEmail(string email)
    {
        var users = await repo.GetAll();
        var user = users.Find(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            throw new UserNotFoundException(email);
        }
        return user;
    }
}