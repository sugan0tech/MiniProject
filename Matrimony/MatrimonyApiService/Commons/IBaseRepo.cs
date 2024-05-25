using MatrimonyApiService.Entities;

namespace MatrimonyApiService.Commons;

public interface IBaseRepo<T> where T : BaseEntity
{
    public Task<T> Add(T ietem);
    public Task<T> DeleteById(int id);
    public Task<T> Update(T entity);
    public Task<T> GetById(int id);
    public Task<List<T>> GetAll();
}