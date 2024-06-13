using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNoteCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Note_CategoryId",
                table: "Note",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_NoteCategory_CategoryId",
                table: "Note",
                column: "CategoryId",
                principalTable: "NoteCategory",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_NoteCategory_CategoryId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_CategoryId",
                table: "Note");
        }
    }
}
