using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddTrailEndedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Profiles",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailEnded",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrailEnded",
                table: "Memberships");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Profiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
