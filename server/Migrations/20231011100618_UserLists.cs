using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UserLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lists_ListID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserLists");

            migrationBuilder.DropIndex(
                name: "IX_Users_ListID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ListID",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Lists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lists_UserID",
                table: "Lists",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_UserID",
                table: "Lists",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Users_UserID",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Lists_UserID",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Lists");

            migrationBuilder.AddColumn<int>(
                name: "ListID",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserLists",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    ListID = table.Column<int>(type: "INTEGER", nullable: false),
                    ListName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLists", x => new { x.UserID, x.ListID });
                    table.ForeignKey(
                        name: "FK_UserLists_Lists_ListID",
                        column: x => x.ListID,
                        principalTable: "Lists",
                        principalColumn: "ListID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLists_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ListID",
                table: "Users",
                column: "ListID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLists_ListID",
                table: "UserLists",
                column: "ListID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lists_ListID",
                table: "Users",
                column: "ListID",
                principalTable: "Lists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
