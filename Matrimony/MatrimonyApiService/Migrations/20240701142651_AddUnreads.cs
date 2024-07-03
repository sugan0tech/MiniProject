using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddUnreads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Unreads",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unreads",
                table: "Chats");
        }
    }
}
