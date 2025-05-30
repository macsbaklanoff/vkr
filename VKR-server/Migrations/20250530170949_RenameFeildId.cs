using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKR_server.Migrations
{
    /// <inheritdoc />
    public partial class RenameFeildId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "groups",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "files",
                newName: "file_id");

            migrationBuilder.RenameColumn(
                name: "EstimationId",
                table: "estimations",
                newName: "estimation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "groups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "files",
                newName: "FileId");

            migrationBuilder.RenameColumn(
                name: "estimation_id",
                table: "estimations",
                newName: "EstimationId");
        }
    }
}
