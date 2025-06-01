using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeFieldEstRecommendationsInEstTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "est_recommendations",
                table: "estimations",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "est_recommendations",
                table: "estimations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
