using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileAndEstimation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estimations_files_FileId",
                table: "estimations");

            migrationBuilder.DropForeignKey(
                name: "FK_files_users_UserId",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_UserId",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_estimations_FileId",
                table: "estimations");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "estimations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "files",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "estimations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_files_UserId",
                table: "files",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_estimations_FileId",
                table: "estimations",
                column: "FileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_estimations_files_FileId",
                table: "estimations",
                column: "FileId",
                principalTable: "files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_files_users_UserId",
                table: "files",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
