using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrimonyApiService.Migrations
{
    /// <inheritdoc />
    public partial class StaffTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staves_Addresses_AddressId1",
                table: "Staves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staves",
                table: "Staves");

            migrationBuilder.RenameTable(
                name: "Staves",
                newName: "Staffs");

            migrationBuilder.RenameIndex(
                name: "IX_Staves_AddressId1",
                table: "Staffs",
                newName: "IX_Staffs_AddressId1");

            migrationBuilder.AlterColumn<string>(
                name: "Religion",
                table: "Preferences",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Occupation",
                table: "Preferences",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "MotherTongue",
                table: "Preferences",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Preferences",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Addresses_AddressId1",
                table: "Staffs",
                column: "AddressId1",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Addresses_AddressId1",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs");

            migrationBuilder.RenameTable(
                name: "Staffs",
                newName: "Staves");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_AddressId1",
                table: "Staves",
                newName: "IX_Staves_AddressId1");

            migrationBuilder.AlterColumn<string>(
                name: "Religion",
                table: "Preferences",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Occupation",
                table: "Preferences",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "MotherTongue",
                table: "Preferences",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Preferences",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staves",
                table: "Staves",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staves_Addresses_AddressId1",
                table: "Staves",
                column: "AddressId1",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
