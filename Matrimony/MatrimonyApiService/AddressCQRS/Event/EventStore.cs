using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MatrimonyApiService.AddressCQRS.Event;

public class EventStore(EventStoreDbContext context) : IEventStore
{
    public async Task SaveEventAsync<TEvent>(TEvent @event)
    {
        var eventEntity = new EventEntity
        {
            EventType = @event.GetType().Name,
            EventData = JsonConvert.SerializeObject(@event),
            CreatedAt = DateTime.UtcNow
        };

        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>()
    {
        var eventTypeName = typeof(TEvent).Name;
        var events = await context.Events
            .Where(e => e.EventType == eventTypeName)
            .ToListAsync();

        return events.Select(e => JsonConvert.DeserializeObject<TEvent>(e.EventData)).ToList();
    }
}