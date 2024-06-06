using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MatrimonyApiService.AddressCQRS.Event;

public class EventStore : IEventStore
{
    private readonly EventStoreDbContext _context;

    public EventStore(EventStoreDbContext context)
    {
        _context = context;
    }

    public async Task SaveEventAsync<TEvent>(TEvent @event)
    {
        var eventEntity = new EventEntity
        {
            EventType = @event.GetType().Name,
            EventData = JsonConvert.SerializeObject(@event),
            CreatedAt = DateTime.UtcNow
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>()
    {
        var eventTypeName = typeof(TEvent).Name;
        var events = await _context.Events
            .Where(e => e.EventType == eventTypeName)
            .ToListAsync();

        return events.Select(e => JsonConvert.DeserializeObject<TEvent>(e.EventData)).ToList();
    }
}