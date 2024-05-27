using AutoMapper;

namespace MatrimonyApiService.Commons;

/// <summary>
/// A base service implementation for entities.
/// </summary>
/// <typeparam name="TBaseEntity">The type of the entity.</typeparam>
public abstract class BaseService<TBaseEntity, TDTO>(IBaseRepo<TBaseEntity> repo, IMapper mapper, ILogger<BaseService<TBaseEntity, TDTO>> logger)
    : IBaseService<TBaseEntity, TDTO> where TBaseEntity : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The entity with the specified identifier.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the entity with the specified identifier is not found.</exception>
    public async Task<TDTO> GetById(int id)
    {
        try
        {
            var entity = await repo.GetById(id);
            return mapper.Map<TDTO>(entity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    public async Task<List<TDTO>> GetAll()
    {
        var entities=  await repo.GetAll();
        return entities.ConvertAll(input => mapper.Map<TDTO>(input)).ToList();
    }

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public async Task<TDTO> Add(TDTO entity)
    {
        var res = await repo.Add(entity);
        return mapper.Map<TDTO>(res);
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="updateEntity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public async Task<TDTO> Update(TBaseEntity updateEntity)
    {
        try
        {
            var entity = await repo.Update(updateEntity);
            return mapper.Map<TDTO>(entity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>The deleted entity.</returns>
    public async Task<TDTO> DeleteById(int id)
    {
        try
        {
            var deletedEntity =  await repo.DeleteById(id);
            return mapper.Map<TDTO>(deletedEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }
}