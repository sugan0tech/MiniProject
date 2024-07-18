using MatrimonyEventConsumer.Models;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyEventConsumer.Repos;

public class ReplicaContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}