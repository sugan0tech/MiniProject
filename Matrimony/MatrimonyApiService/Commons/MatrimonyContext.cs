using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Commons;

public class MatrimonyContext(DbContextOptions options) : DbContext(options)
{
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
        #region User

        modelBuilder.Entity<User.User>().HasOne<Address.Address>(user => user.Address);
        modelBuilder.Entity<User.User>()
            .HasMany<Message.Message>(user => user.Messages)
            .WithOne(message => message.Sender)
            .HasForeignKey(message => message.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User.User>()
            .HasMany<ProfileView.ProfileView>(user => user.Views)
            .WithOne(message => message.Viewer)
            .HasForeignKey(view => view.Viewer)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User.User>().Navigation<Address.Address>(user => user.Address).AutoInclude();

        #endregion

        #region Profile

        #region Membership

        modelBuilder.Entity<Profile.Profile>()
            .HasOne<Membership.Membership>(profile => profile.Membership)
            .WithOne(membership => membership.Profile)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Profile.Profile>()
            .Navigation(profile => profile.Membership)
            .AutoInclude();

        #endregion

        #region ProfileView

        modelBuilder.Entity<Profile.Profile>()
            .HasMany<ProfileView.ProfileView>(profile => profile.ProfileViews)
            .WithOne(view => view.ViewedAtProfile)
            .HasForeignKey(view => view.ViewedProfileAt)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Profile.Profile>()
            .Navigation(profile => profile.ProfileViews)
            .AutoInclude();

        #endregion

        #region Match

        modelBuilder.Entity<Profile.Profile>()
            .HasMany<Match.Match>(profile => profile.Matches)
            .WithOne(match => match.ProfileOne)
            .HasForeignKey(match => match.ProfileOne)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Profile.Profile>()
            .HasMany<Match.Match>(profile => profile.Matches)
            .WithOne(match => match.ProfileTwo)
            .HasForeignKey(match => match.ProfileTwoId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Preference

        modelBuilder.Entity<Profile.Profile>()
            .HasOne<Preference.Preference>()
            .WithOne(preference => preference.PreferenceFor)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Profile.Profile>()
            .Navigation(profile => profile.Preference)
            .AutoInclude();

        #endregion

        #endregion

        #region ProfileView

        modelBuilder.Entity<ProfileView.ProfileView>();

        #endregion

        #region Message

        modelBuilder.Entity<Message.Message>();

        #endregion

        #region Perference

        modelBuilder.Entity<Preference.Preference>();

        #endregion

        #region Staff

        modelBuilder.Entity<Staff.Staff>()
            .Navigation(staff => staff.Address)
            .AutoInclude();

        #endregion

        #region Address

        modelBuilder.Entity<Address.Address>();

        #endregion

        #region Match

        modelBuilder.Entity<Match.Match>();

        #endregion

        #region Membership

        modelBuilder.Entity<Membership.Membership>();

        #endregion
    }
}