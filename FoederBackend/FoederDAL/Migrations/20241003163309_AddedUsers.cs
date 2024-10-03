using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoederDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Households_HouseholdId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_HouseholdId",
                table: "Users",
                newName: "IX_Users_HouseholdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Households_HouseholdId",
                table: "Users",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Households_HouseholdId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HouseholdId",
                table: "User",
                newName: "IX_User_HouseholdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Households_HouseholdId",
                table: "User",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
