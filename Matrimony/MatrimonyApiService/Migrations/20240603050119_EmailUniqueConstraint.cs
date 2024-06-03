using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class EmailUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Email_Ind",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "Email_Ind",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Email_Ind",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "Email_Ind",
                table: "Users",
                column: "Email");
        }
    }
}
