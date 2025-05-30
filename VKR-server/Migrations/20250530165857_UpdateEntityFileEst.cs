using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityFileEst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estimations_files_file_id",
                table: "estimations");

            migrationBuilder.DropIndex(
                name: "IX_estimations_file_id",
                table: "estimations");

            migrationBuilder.DropColumn(
                name: "file_id",
                table: "estimations");

            migrationBuilder.AddColumn<int>(
                name: "estimation_id",
                table: "files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_files_estimation_id",
                table: "files",
                column: "estimation_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files",
                column: "estimation_id",
                principalTable: "estimations",
                principalColumn: "EstimationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_estimation_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "estimation_id",
                table: "files");

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
        }
    }
}
