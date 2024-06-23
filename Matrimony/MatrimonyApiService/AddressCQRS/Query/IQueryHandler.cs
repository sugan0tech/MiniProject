namespace MatrimonyApiService.AddressCQRS.Query;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Handle(TQuery query);
}