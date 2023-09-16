using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RepairedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Us_UserID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Us_User",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Us_User",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Us_Lists_ListID",
                table: "Us");

            migrationBuilder.DropForeignKey(
                name: "FK_Verifiers_Us_UserID",
                table: "Verifiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Us",
                table: "Us");

            migrationBuilder.RenameTable(
                name: "Us",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Us_Username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Us_ListID",
                table: "Users",
                newName: "IX_Users_ListID");

            migrationBuilder.RenameIndex(
                name: "IX_Us_Email",
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
                newName: "Us");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "Us",
                newName: "IX_Us_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ListID",
                table: "Us",
                newName: "IX_Us_ListID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Us",
                newName: "IX_Us_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Us",
                table: "Us",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Us_UserID",
                table: "Artists",
                column: "UserID",
                principalTable: "Us",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Us_User",
                table: "Orders",
                column: "User",
                principalTable: "Us",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Us_User",
                table: "Reviews",
                column: "User",
                principalTable: "Us",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Us_Lists_ListID",
                table: "Us",
                column: "ListID",
                principalTable: "Lists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifiers_Us_UserID",
                table: "Verifiers",
                column: "UserID",
                principalTable: "Us",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
