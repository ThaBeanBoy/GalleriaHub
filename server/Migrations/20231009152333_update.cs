using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryFiles_Files_FileID",
                table: "GalleryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryFiles_Galleries_GalleryID",
                table: "GalleryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFiles_Files_FileID",
                table: "ProductFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLists_Files_FileID",
                table: "UserLists");

            migrationBuilder.DropIndex(
                name: "IX_UserLists_FileID",
                table: "UserLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFiles",
                table: "ProductFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProductFiles_FileID",
                table: "ProductFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GalleryFiles",
                table: "GalleryFiles");

            migrationBuilder.DropColumn(
                name: "FileID",
                table: "UserLists");

            migrationBuilder.RenameColumn(
                name: "FileID",
                table: "ProductFiles",
                newName: "ProductFileID");

            migrationBuilder.RenameColumn(
                name: "FileID",
                table: "GalleryFiles",
                newName: "GalleryID1");

            migrationBuilder.RenameIndex(
                name: "IX_GalleryFiles_FileID",
                table: "GalleryFiles",
                newName: "IX_GalleryFiles_GalleryID1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductFileID",
                table: "ProductFiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "GalleryID",
                table: "GalleryFiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFiles",
                table: "ProductFiles",
                column: "ProductFileID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GalleryFiles",
                table: "GalleryFiles",
                column: "GalleryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFiles_ProductID",
                table: "ProductFiles",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryFiles_Galleries_GalleryID1",
                table: "GalleryFiles",
                column: "GalleryID1",
                principalTable: "Galleries",
                principalColumn: "GalleryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryFiles_Galleries_GalleryID1",
                table: "GalleryFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFiles",
                table: "ProductFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProductFiles_ProductID",
                table: "ProductFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GalleryFiles",
                table: "GalleryFiles");

            migrationBuilder.RenameColumn(
                name: "ProductFileID",
                table: "ProductFiles",
                newName: "FileID");

            migrationBuilder.RenameColumn(
                name: "GalleryID1",
                table: "GalleryFiles",
                newName: "FileID");

            migrationBuilder.RenameIndex(
                name: "IX_GalleryFiles_GalleryID1",
                table: "GalleryFiles",
                newName: "IX_GalleryFiles_FileID");

            migrationBuilder.AddColumn<int>(
                name: "FileID",
                table: "UserLists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FileID",
                table: "ProductFiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "GalleryID",
                table: "GalleryFiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFiles",
                table: "ProductFiles",
                columns: new[] { "ProductID", "FileID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GalleryFiles",
                table: "GalleryFiles",
                columns: new[] { "GalleryID", "FileID" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLists_FileID",
                table: "UserLists",
                column: "FileID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFiles_FileID",
                table: "ProductFiles",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryFiles_Files_FileID",
                table: "GalleryFiles",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "FileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryFiles_Galleries_GalleryID",
                table: "GalleryFiles",
                column: "GalleryID",
                principalTable: "Galleries",
                principalColumn: "GalleryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFiles_Files_FileID",
                table: "ProductFiles",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "FileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLists_Files_FileID",
                table: "UserLists",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "FileID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
