using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Commons;

/// <summary>
/// A base repository implementation for entities.
/// Repo version 2 , Async improvement from older version.
/// Just changed all the .Result call with await and
/// removed locks ( since not needed for EF )
/// </summary>
/// <typeparam name="TBaseEntity">The type of the entity.</typeparam>
public abstract class BaseRepo<TBaseEntity>(MatrimonyContext context)
    : IBaseRepo<TBaseEntity> where TBaseEntity : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The entity with the specified identifier.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the entity with the specified identifier is not found.</exception>
    public async Task<TBaseEntity> GetById(int id)
    {
        var entity = await context.Set<TBaseEntity>().SingleOrDefaultAsync(entity => entity.Id.Equals(id));
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TBaseEntity).Name} with key {id} not found!!!");
        return entity;
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    public Task<List<TBaseEntity>> GetAll()
    {
        return context.Set<TBaseEntity>().ToListAsync();
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
            throw new ArgumentNullException(nameof(entity), $"{typeof(TBaseEntity).Name} cannot be null.");

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="updateEntity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public async Task<TBaseEntity> Update(TBaseEntity updateEntity)
    {
        var entity = await GetById(updateEntity.Id);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    public async Task<TBaseEntity> DeleteById(int id)
    {
        var entity = await GetById(id);
        context.Set<TBaseEntity>().Remove(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}