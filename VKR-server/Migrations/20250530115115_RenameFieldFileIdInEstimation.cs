using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldFileIdInEstimation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estimations_files_FileId",
                table: "estimations");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "estimations",
                newName: "file_id");

            migrationBuilder.RenameIndex(
                name: "IX_estimations_FileId",
                table: "estimations",
                newName: "IX_estimations_file_id");

            migrationBuilder.AddForeignKey(
                name: "FK_estimations_files_file_id",
                table: "estimations",
                column: "file_id",
                principalTable: "files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estimations_files_file_id",
                table: "estimations");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "estimations",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_estimations_file_id",
                table: "estimations",
                newName: "IX_estimations_FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_estimations_files_FileId",
                table: "estimations",
                column: "FileId",
                principalTable: "files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
