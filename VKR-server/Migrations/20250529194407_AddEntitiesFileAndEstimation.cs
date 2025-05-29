using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitiesFileAndEstimation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estimations",
                columns: table => new
                {
                    EstimationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    est_content = table.Column<int>(type: "integer", nullable: false),
                    est_relevance = table.Column<int>(type: "integer", nullable: false),
                    est_stylistic = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estimations", x => x.EstimationId);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    name_job = table.Column<string>(type: "text", nullable: false),
                    academic_subject = table.Column<string>(type: "text", nullable: false),
                    topic_work = table.Column<string>(type: "text", nullable: false),
                    estimation_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_files_estimations_estimation_id",
                        column: x => x.estimation_id,
                        principalTable: "estimations",
                        principalColumn: "EstimationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_files_estimation_id",
                table: "files",
                column: "estimation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "estimations");
        }
    }
}
