using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_project_Inforce_backend.Migrations
{
    /// <inheritdoc />
    public partial class TryAddForeignKeyToPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoData",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PrewievData",
                table: "Photos");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "PhotoData",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrewievData",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
