using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldUserIdIdInFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_users_UserId",
                table: "files");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "files",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_files_UserId",
                table: "files",
                newName: "IX_files_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_files_users_user_id",
                table: "files",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_users_user_id",
                table: "files");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "files",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_files_user_id",
                table: "files",
                newName: "IX_files_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_files_users_UserId",
                table: "files",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
