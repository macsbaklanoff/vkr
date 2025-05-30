using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldEstIdNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files");

            migrationBuilder.AlterColumn<int>(
                name: "estimation_id",
                table: "files",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files",
                column: "estimation_id",
                principalTable: "estimations",
                principalColumn: "EstimationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files");

            migrationBuilder.AlterColumn<int>(
                name: "estimation_id",
                table: "files",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_files_estimations_estimation_id",
                table: "files",
                column: "estimation_id",
                principalTable: "estimations",
                principalColumn: "EstimationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
