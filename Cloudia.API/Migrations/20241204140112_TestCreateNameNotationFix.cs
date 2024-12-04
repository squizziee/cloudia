using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloudia.API.Migrations
{
    /// <inheritdoc />
    public partial class TestCreateNameNotationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Posts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Posts",
                newName: "user_profile_id");

            migrationBuilder.RenameColumn(
                name: "TextContent",
                table: "Posts",
                newName: "text_content");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "Posts",
                newName: "posted_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Posts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_profile_id",
                table: "Posts",
                newName: "UserProfileId");

            migrationBuilder.RenameColumn(
                name: "text_content",
                table: "Posts",
                newName: "TextContent");

            migrationBuilder.RenameColumn(
                name: "posted_at",
                table: "Posts",
                newName: "PostedAt");
        }
    }
}
