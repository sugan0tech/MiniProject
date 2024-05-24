using MatrimonyApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Contexts;

public class MatrimonyContext: DbContext
{
    public MatrimonyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfileView> ProfileViews { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<Staff> Staves { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}