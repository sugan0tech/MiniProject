namespace MatrimonyEventConsumer.Repos;

public interface IBaseRepo<T>
{
    public Task<T> Add(T ietem);
    public Task<T> DeleteById(int id);
    public Task<T> Update(T entity);
    public Task<T> GetById(int id);
    public Task<List<T>> GetAll();
}