using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityFileEstimation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "estimations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estimations_files_FileId",
                table: "estimations");

            migrationBuilder.DropIndex(
                name: "IX_estimations_FileId",
                table: "estimations");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "estimations");
        }
    }
}
