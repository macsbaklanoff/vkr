using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class EntityFileEst : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "files",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "file_id",
                table: "estimations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_estimations_file_id",
                table: "estimations",
                column: "file_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_estimations_files_file_id",
                table: "estimations",
                column: "file_id",
                principalTable: "files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_estimations_files_file_id",
                table: "estimations");

            migrationBuilder.DropForeignKey(
                name: "FK_files_users_user_id",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_estimations_file_id",
                table: "estimations");

            migrationBuilder.DropColumn(
                name: "file_id",
                table: "estimations");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "files",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_files_user_id",
                table: "files",
                newName: "IX_files_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "files",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_files_users_UserId",
                table: "files",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
