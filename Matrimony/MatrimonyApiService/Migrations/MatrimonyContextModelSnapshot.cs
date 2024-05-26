﻿// <auto-generated />
using System;
using MatrimonyApiService.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    [DbContext(typeof(MatrimonyContext))]
    partial class MatrimonyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MatrimonyApiService.Address.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Street")
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("MatrimonyApiService.Match.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FoundAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<bool>("ProfileOneLike")
                        .HasColumnType("bit");

                    b.Property<bool>("ProfileTwoLike")
                        .HasColumnType("bit");

                    b.Property<int>("ReceivedProfileId")
                        .HasColumnType("int");

                    b.Property<int>("SentProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceivedProfileId");

                    b.HasIndex("SentProfileId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("MatrimonyApiService.Membership.Membership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("EndsAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsTrail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTrailEnded")
                        .HasColumnType("bit");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("MatrimonyApiService.Message.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MatrimonyApiService.Preference.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("MaxAge")
                        .HasColumnType("int");

                    b.Property<int>("MaxHeight")
                        .HasColumnType("int");

                    b.Property<int>("MinAge")
                        .HasColumnType("int");

                    b.Property<int>("MinHeight")
                        .HasColumnType("int");

                    b.Property<string>("MotherTongue")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Occupation")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("PreferenceForId")
                        .HasColumnType("int");

                    b.Property<string>("Religion")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("MatrimonyApiService.Profile.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnnualIncome")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Ethnicity")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Habit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("ManagedById")
                        .HasColumnType("int");

                    b.Property<string>("ManagedByRelation")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("MaritalStatus")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("MembershipId")
                        .HasColumnType("int");

                    b.Property<string>("MotherTongue")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Occupation")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Religion")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ManagedById");

                    b.HasIndex("MembershipId")
                        .IsUnique();

                    b.HasIndex("PreferenceId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("MatrimonyApiService.ProfileView.ProfileView", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ViewedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ViewedProfileAt")
                        .HasColumnType("int");

                    b.Property<int>("ViewerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ViewedProfileAt");

                    b.HasIndex("ViewerId");

                    b.ToTable("ProfileViews");
                });

            modelBuilder.Entity("MatrimonyApiService.Staff.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId1")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("HashKey")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<byte[]>("Password")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId1");

                    b.HasIndex(new[] { "Email" }, "Email_Ind");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("MatrimonyApiService.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId1")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("HashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId1");

                    b.HasIndex(new[] { "Email" }, "Email_Ind");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MatrimonyApiService.Match.Match", b =>
                {
                    b.HasOne("MatrimonyApiService.Profile.Profile", "ReceivedProfile")
                        .WithMany("ReceivedMatches")
                        .HasForeignKey("ReceivedProfileId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.Profile.Profile", "SentProfile")
                        .WithMany("SentMatches")
                        .HasForeignKey("SentProfileId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ReceivedProfile");

                    b.Navigation("SentProfile");
                });

            modelBuilder.Entity("MatrimonyApiService.Message.Message", b =>
                {
                    b.HasOne("MatrimonyApiService.User.User", "Receiver")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.User.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MatrimonyApiService.Profile.Profile", b =>
                {
                    b.HasOne("MatrimonyApiService.User.User", "ManagedBy")
                        .WithMany()
                        .HasForeignKey("ManagedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.Membership.Membership", "Membership")
                        .WithOne("Profile")
                        .HasForeignKey("MatrimonyApiService.Profile.Profile", "MembershipId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.Preference.Preference", "Preference")
                        .WithOne("PreferenceFor")
                        .HasForeignKey("MatrimonyApiService.Profile.Profile", "PreferenceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.User.User", "User")
                        .WithOne()
                        .HasForeignKey("MatrimonyApiService.Profile.Profile", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ManagedBy");

                    b.Navigation("Membership");

                    b.Navigation("Preference");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MatrimonyApiService.ProfileView.ProfileView", b =>
                {
                    b.HasOne("MatrimonyApiService.Profile.Profile", "ViewedAtProfile")
                        .WithMany("ProfileViews")
                        .HasForeignKey("ViewedProfileAt")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MatrimonyApiService.User.User", "Viewer")
                        .WithMany("Views")
                        .HasForeignKey("ViewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ViewedAtProfile");

                    b.Navigation("Viewer");
                });

            modelBuilder.Entity("MatrimonyApiService.Staff.Staff", b =>
                {
                    b.HasOne("MatrimonyApiService.Address.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId1");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MatrimonyApiService.User.User", b =>
                {
                    b.HasOne("MatrimonyApiService.Address.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId1");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MatrimonyApiService.Membership.Membership", b =>
                {
                    b.Navigation("Profile");
                });

            modelBuilder.Entity("MatrimonyApiService.Preference.Preference", b =>
                {
                    b.Navigation("PreferenceFor");
                });

            modelBuilder.Entity("MatrimonyApiService.Profile.Profile", b =>
                {
                    b.Navigation("ProfileViews");

                    b.Navigation("ReceivedMatches");

                    b.Navigation("SentMatches");
                });

            modelBuilder.Entity("MatrimonyApiService.User.User", b =>
                {
                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");

                    b.Navigation("Views");
                });
#pragma warning restore 612, 618
        }
    }
}
