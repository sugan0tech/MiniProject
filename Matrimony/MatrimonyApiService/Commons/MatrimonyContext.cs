using MatrimonyApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Commons;

public class MatrimonyContext: DbContext
{
    public MatrimonyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User.User> Users { get; set; }
    public DbSet<Profile.Profile> Profiles { get; set; }
    public DbSet<ProfileView.ProfileView> ProfileViews { get; set; }
    public DbSet<Message.Message> Messages { get; set; }
    public DbSet<Preference.Preference> Preferences { get; set; }
    public DbSet<Staff.Staff> Staves { get; set; }
    public DbSet<Address.Address> Addresses { get; set; }
    public DbSet<Match.Match> Matches { get; set; }
    public DbSet<Membership.Membership> Memberships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}