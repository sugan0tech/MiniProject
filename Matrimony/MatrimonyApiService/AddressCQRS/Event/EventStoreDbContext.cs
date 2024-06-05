using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.AddressCQRS;

public class EventStoreDbContext : DbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<EventEntity> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventEntity>().ToTable("Events");
    }
}