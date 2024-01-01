using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittlePictureNetworkBackend.Migrations
{
    /// <inheritdoc />
    public partial class MasiveChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AlbumId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PhotosId",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_PhotosId",
                table: "Albums",
                column: "PhotosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums",
                column: "PhotosId",
                principalTable: "EFGuidCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_PhotosId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "PhotosId",
                table: "Albums");

            migrationBuilder.AddColumn<Guid>(
                name: "AlbumId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AlbumId",
                table: "Photos",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId");
        }
    }
}
