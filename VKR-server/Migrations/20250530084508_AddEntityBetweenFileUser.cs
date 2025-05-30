using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityBetweenFileUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_files_UserId",
                table: "files",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_files_users_UserId",
                table: "files",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_users_UserId",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_UserId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "files");
        }
    }
}
