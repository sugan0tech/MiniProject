using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class NullablePreferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_PreferenceId",
                table: "Profiles");

            migrationBuilder.AlterColumn<int>(
                name: "PreferenceId",
                table: "Profiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PreferenceId",
                table: "Profiles",
                column: "PreferenceId",
                unique: true,
                filter: "[PreferenceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_PreferenceId",
                table: "Profiles");

            migrationBuilder.AlterColumn<int>(
                name: "PreferenceId",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PreferenceId",
                table: "Profiles",
                column: "PreferenceId",
                unique: true);
        }
    }
}
