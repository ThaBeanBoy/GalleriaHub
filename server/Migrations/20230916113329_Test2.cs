using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_User_UserID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_User_User",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_User",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Lists_ListID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Verifiers_User_UserID",
                table: "Verifiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_Username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "IX_User_ListID",
                table: "Users",
                newName: "IX_Users_ListID");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_UserID",
                table: "Artists",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_User",
                table: "Orders",
                column: "User",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_User",
                table: "Reviews",
                column: "User",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lists_ListID",
                table: "Users",
                column: "ListID",
                principalTable: "Lists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifiers_Users_UserID",
                table: "Verifiers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_User",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_User",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lists_ListID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Verifiers_Users_UserID",
                table: "Verifiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "User",
                newName: "IX_User_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ListID",
                table: "User",
                newName: "IX_User_ListID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_User_UserID",
                table: "Artists",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_User_User",
                table: "Orders",
                column: "User",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_User",
                table: "Reviews",
                column: "User",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Lists_ListID",
                table: "User",
                column: "ListID",
                principalTable: "Lists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifiers_User_UserID",
                table: "Verifiers",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
