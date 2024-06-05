namespace MatrimonyApiService.AddressCQRS;

public interface IEventStore
{
    Task SaveEventAsync<TEvent>(TEvent @event);
    Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>();
}