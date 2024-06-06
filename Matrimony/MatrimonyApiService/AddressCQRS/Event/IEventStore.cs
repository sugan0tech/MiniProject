namespace MatrimonyApiService.AddressCQRS.Event;

public interface IEventStore
{
    Task SaveEventAsync<TEvent>(TEvent @event);
    Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>();
}