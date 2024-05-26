using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddHabitsStr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Habits",
                table: "Profiles");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "HashKey",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Habit",
                table: "Profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Habit",
                table: "Profiles");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "HashKey",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Habits",
                table: "Profiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
