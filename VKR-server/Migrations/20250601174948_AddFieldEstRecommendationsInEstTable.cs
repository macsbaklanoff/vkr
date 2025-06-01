using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldEstRecommendationsInEstTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "est_recommendations",
                table: "estimations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "est_recommendations",
                table: "estimations");
        }
    }
}
