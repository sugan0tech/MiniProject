using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class ProfileViewAndMatchRequestFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileViews_Profiles_ViewedProfileAt",
                table: "ProfileViews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileViews_Users_ViewerId",
                table: "ProfileViews");

            migrationBuilder.RenameColumn(
                name: "ProfileTwoLike",
                table: "Matches",
                newName: "ReceiverLike");

            migrationBuilder.RenameColumn(
                name: "ProfileOneLike",
                table: "Matches",
                newName: "IsRejected");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileViews_Profiles_ViewedProfileAt",
                table: "ProfileViews",
                column: "ViewedProfileAt",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileViews_Profiles_ViewerId",
                table: "ProfileViews",
                column: "ViewerId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileViews_Profiles_ViewedProfileAt",
                table: "ProfileViews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileViews_Profiles_ViewerId",
                table: "ProfileViews");

            migrationBuilder.RenameColumn(
                name: "ReceiverLike",
                table: "Matches",
                newName: "ProfileTwoLike");

            migrationBuilder.RenameColumn(
                name: "IsRejected",
                table: "Matches",
                newName: "ProfileOneLike");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileViews_Profiles_ViewedProfileAt",
                table: "ProfileViews",
                column: "ViewedProfileAt",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileViews_Users_ViewerId",
                table: "ProfileViews",
                column: "ViewerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
