namespace MatrimonyApiService.Commons;

public interface IBaseService<T, DTO>
{
    public Task<DTO> Add(DTO item);
    public Task<DTO> DeleteById(int id);
    public Task<DTO> Update(DTO entity);
    public Task<DTO> GetById(int id);
    public Task<List<DTO>> GetAll();
}