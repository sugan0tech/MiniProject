using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Profiles_SenderId",
                table: "Chats");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Profiles_SenderId",
                table: "Chats",
                column: "SenderId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Profiles_SenderId",
                table: "Chats");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Profiles_SenderId",
                table: "Chats",
                column: "SenderId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
