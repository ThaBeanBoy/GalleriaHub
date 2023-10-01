using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class StructureUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistVerifications_Artists_ArtistID",
                table: "ArtistVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Artists_ArtistID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.RenameColumn(
                name: "ArtistID",
                table: "Products",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ArtistID",
                table: "Products",
                newName: "IX_Products_UserID");

            migrationBuilder.RenameColumn(
                name: "ArtistID",
                table: "ArtistVerifications",
                newName: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistVerifications_Users_UserID",
                table: "ArtistVerifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserID",
                table: "Products",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistVerifications_Users_UserID",
                table: "ArtistVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Products",
                newName: "ArtistID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UserID",
                table: "Products",
                newName: "IX_Products_ArtistID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ArtistVerifications",
                newName: "ArtistID");

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistID);
                    table.UniqueConstraint("AK_Artists_UserID", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Artists_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistVerifications_Artists_ArtistID",
                table: "ArtistVerifications",
                column: "ArtistID",
                principalTable: "Artists",
                principalColumn: "ArtistID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Artists_ArtistID",
                table: "Products",
                column: "ArtistID",
                principalTable: "Artists",
                principalColumn: "ArtistID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
