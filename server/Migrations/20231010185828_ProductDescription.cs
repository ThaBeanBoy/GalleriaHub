using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ProductDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Files_FileID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FileID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileID",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "FileID",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FileID",
                table: "Products",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Files_FileID",
                table: "Products",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "FileID");
        }
    }
}
