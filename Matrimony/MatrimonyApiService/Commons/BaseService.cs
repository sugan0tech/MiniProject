namespace MatrimonyApiService.Commons;

/// <summary>
/// A base service implementation for entities.
/// </summary>
/// <typeparam name="TBaseEntity">The type of the entity.</typeparam>
public abstract class BaseService<TBaseEntity>(IBaseRepo<TBaseEntity> repo, ILogger<BaseService<TBaseEntity>> logger)
    : IBaseService<TBaseEntity> where TBaseEntity : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The entity with the specified identifier.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the entity with the specified identifier is not found.</exception>
    public async Task<TBaseEntity> GetById(int id)
    {
        try
        {
            return await repo.GetById(id);
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
    public async Task<List<TBaseEntity>> GetAll()
    {
        return await repo.GetAll();
    }

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public async Task<TBaseEntity> Add(TBaseEntity entity)
    {
        if (entity == null)
        {
            logger.LogError($"Cant save on null for {typeof(TBaseEntity).Name}");
            throw new ArgumentNullException($"Cant save on null for {typeof(TBaseEntity).Name}");
        }

        return await repo.Add(entity);
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="updateEntity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public async Task<TBaseEntity> Update(TBaseEntity updateEntity)
    {
        try
        {
            return await repo.Update(updateEntity);
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
    public async Task<TBaseEntity> DeleteById(int id)
    {
        try
        {
            return await repo.DeleteById(id);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }
}