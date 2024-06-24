using MatrimonyApiService.Commons.Enums;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Commons;

public class MatrimonyContext(DbContextOptions<MatrimonyContext> options) : DbContext(options)
{
    public DbSet<User.User> Users { get; set; }
    public DbSet<UserSession.UserSession> UserSessions { get; set; }
    public DbSet<Profile.Profile> Profiles { get; set; }
    public DbSet<ProfileView.ProfileView> ProfileViews { get; set; }
    public DbSet<Message.Message> Messages { get; set; }
    public DbSet<Preference.Preference> Preferences { get; set; }
    public DbSet<Staff.Staff> Staffs { get; set; }
    public DbSet<Address.Address> Addresses { get; set; }
    public DbSet<MatchRequest.MatchRequest> Matches { get; set; }
    public DbSet<Membership.Membership> Memberships { get; set; }
    public DbSet<Report.Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Address

        modelBuilder.Entity<Address.Address>()
            .HasOne<User.User>(address => address.User)
            .WithOne(user => user.Address)
            .HasForeignKey<User.User>(user => user.AddressId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region User

        modelBuilder.Entity<User.User>()
            .HasOne<Address.Address>(user => user.Address)
            .WithOne(address => address.User)
            .HasForeignKey<Address.Address>(address => address.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User.User>()
            .HasMany<Message.Message>(user => user.MessagesSent)
            .WithOne(message => message.Sender)
            .HasForeignKey(message => message.SenderId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User.User>()
            .HasMany<Message.Message>(user => user.MessagesReceived)
            .WithOne(message => message.Receiver)
            .HasForeignKey(message => message.ReceiverId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User.User>().Navigation<Address.Address>(user => user.Address).AutoInclude();

        #endregion

        #region UserSession

        modelBuilder.Entity<UserSession.UserSession>();

        #endregion

        #region Profile

        #region User

        modelBuilder.Entity<Profile.Profile>()
            .HasOne<User.User>(profile => profile.ManagedBy)
            .WithMany()
            .HasForeignKey(profile => profile.ManagedById)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Profile.Profile>()
            .HasOne<User.User>(profile => profile.User)
            .WithOne()
            .HasForeignKey<Profile.Profile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Membership

        modelBuilder.Entity<Profile.Profile>()
            .HasOne<Membership.Membership>(profile => profile.Membership)
            .WithOne(membership => membership.Profile)
            .HasForeignKey<Membership.Membership>(membership => membership.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region ProfileView

        modelBuilder.Entity<Profile.Profile>()
            .HasMany<ProfileView.ProfileView>(profile => profile.ProfileViews)
            .WithOne(view => view.ViewedAtProfile)
            .HasForeignKey(view => view.ViewedProfileAt)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Profile.Profile>()
            .Navigation(profile => profile.ProfileViews)
            .AutoInclude();
        modelBuilder.Entity<Profile.Profile>()
            .HasMany<ProfileView.ProfileView>(profile => profile.Views)
            .WithOne(view => view.Viewer)
            .HasForeignKey(view => view.ViewerId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region MatchRequest

        modelBuilder.Entity<Profile.Profile>()
            .HasMany<MatchRequest.MatchRequest>(profile => profile.SentMatches)
            .WithOne(match => match.SentProfile)
            .HasForeignKey(match => match.SentProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Profile.Profile>()
            .HasMany<MatchRequest.MatchRequest>(profile => profile.ReceivedMatches)
            .WithOne(match => match.ReceivedProfile)
            .HasForeignKey(match => match.ReceivedProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Preference

        modelBuilder.Entity<Profile.Profile>()
            .HasOne<Preference.Preference>(profile => profile.Preference)
            .WithOne(preference => preference.PreferenceFor)
            .HasForeignKey<Preference.Preference>(preference => preference.PreferenceForId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #endregion

        #region Report

        modelBuilder.Entity<Report.Report>();

        #endregion

        #region ProfileView

        modelBuilder.Entity<ProfileView.ProfileView>();

        #endregion

        #region Message

        modelBuilder.Entity<Message.Message>()
            .HasOne<User.User>(message => message.Sender)
            .WithMany(user => user.MessagesSent)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Message.Message>()
            .HasOne<User.User>(message => message.Receiver)
            .WithMany(user => user.MessagesReceived)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Perference

        modelBuilder.Entity<Preference.Preference>()
            .HasOne<Profile.Profile>(preference => preference.PreferenceFor)
            .WithOne(profile => profile.Preference)
            .HasForeignKey<Profile.Profile>(profile => profile.PreferenceId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Staff

        modelBuilder.Entity<Staff.Staff>()
            .HasOne<Address.Address>(staff => staff.Address);
        modelBuilder.Entity<Staff.Staff>()
            .Navigation<Address.Address>(staff => staff.Address)
            .AutoInclude();

        #endregion

        #region MatchRequest

        modelBuilder.Entity<MatchRequest.MatchRequest>()
            .HasOne<Profile.Profile>(match => match.SentProfile)
            .WithMany(profile => profile.SentMatches)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<MatchRequest.MatchRequest>()
            .HasOne<Profile.Profile>(match => match.ReceivedProfile)
            .WithMany(profile => profile.ReceivedMatches)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Membership

        modelBuilder.Entity<Membership.Membership>()
            .HasOne<Profile.Profile>(membership => membership.Profile)
            .WithOne(profile => profile.Membership)
            .HasForeignKey<Profile.Profile>(profile => profile.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }
}