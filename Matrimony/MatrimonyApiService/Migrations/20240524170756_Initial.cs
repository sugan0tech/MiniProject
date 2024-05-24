using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsTrail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotherTongue = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Religion = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Education = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MinHeight = table.Column<int>(type: "int", nullable: false),
                    MaxHeight = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    PreferenceForId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    HashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    AddressId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staves_Addresses_AddressId1",
                        column: x => x.AddressId1,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    HashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    AddressId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId1",
                        column: x => x.AddressId1,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AnnualIncome = table.Column<int>(type: "int", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MotherTongue = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Religion = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Ethnicity = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Habits = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    ManagedById = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagedByRelation = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_ManagedById",
                        column: x => x.ManagedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileOneId = table.Column<int>(type: "int", nullable: false),
                    ProfileOneLike = table.Column<bool>(type: "bit", nullable: false),
                    ProfileTwoId = table.Column<int>(type: "int", nullable: false),
                    ProfileTwoLike = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    FoundAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Profiles_ProfileOneId",
                        column: x => x.ProfileOneId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Profiles_ProfileTwoId",
                        column: x => x.ProfileTwoId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewerId = table.Column<int>(type: "int", nullable: false),
                    ViewedProfileAt = table.Column<int>(type: "int", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileViews_Profiles_ViewedProfileAt",
                        column: x => x.ViewedProfileAt,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileViews_Users_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ProfileOneId",
                table: "Matches",
                column: "ProfileOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ProfileTwoId",
                table: "Matches",
                column: "ProfileTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ManagedById",
                table: "Profiles",
                column: "ManagedById");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_MembershipId",
                table: "Profiles",
                column: "MembershipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PreferenceId",
                table: "Profiles",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileViews_ViewedProfileAt",
                table: "ProfileViews",
                column: "ViewedProfileAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileViews_ViewerId",
                table: "ProfileViews",
                column: "ViewerId");

            migrationBuilder.CreateIndex(
                name: "Email_Ind",
                table: "Staves",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Staves_AddressId1",
                table: "Staves",
                column: "AddressId1");

            migrationBuilder.CreateIndex(
                name: "Email_Ind",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId1",
                table: "Users",
                column: "AddressId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProfileViews");

            migrationBuilder.DropTable(
                name: "Staves");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
