using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;

namespace MatrimonyApiService.User;

public class UserService(
    IBaseRepo<User> repo,
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserDto> GetById(int userId)
    {
        try
        {
            var user = await repo.GetById(userId);
            return mapper.Map<UserDto>(user);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<List<UserDto>> GetAll()
    {
        var users = await repo.GetAll();
        return users.ConvertAll(input => mapper.Map<UserDto>(input)).ToList();
    }

    /// <intheritdoc/>
    public async Task<UserDto> Add(User user)
    {
        user.IsVerified = false;
        var usr = await repo.Add(user);
        return mapper.Map<UserDto>(usr);
    }

    /// <intheritdoc/>
    public async Task<UserDto> Update(User user)
    {
        try
        {
            var usr = await repo.Update(user);
            return mapper.Map<UserDto>(usr);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<User> GetByEmail(string email)
    {
        var users = await repo.GetAll();
        var user = users.Find(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user == null) throw new UserNotFoundException(email);
        return user;
    }

    /// <intheritdoc/>
    public async Task<UserDto> Validate(int userId, bool status)
    {
        try
        {
            var user = await repo.GetById(userId);
            user.IsVerified = status;
            return await Update(user);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <intheritdoc/>
    public async Task<UserDto> DeleteById(int id)
    {
        try
        {
            return mapper.Map<UserDto>(await repo.DeleteById(id));
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}