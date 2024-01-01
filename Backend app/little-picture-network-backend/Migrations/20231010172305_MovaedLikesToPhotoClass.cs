using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittlePictureNetworkBackend.Migrations
{
    /// <inheritdoc />
    public partial class MovaedLikesToPhotoClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Photos",
                newName: "LikesCount");

            migrationBuilder.RenameColumn(
                name: "Dislikes",
                table: "Photos",
                newName: "DislikesCount");

            migrationBuilder.AddColumn<int>(
                name: "DisikesListId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikesListId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EFGuidCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFGuidCollection", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_DisikesListId",
                table: "Photos",
                column: "DisikesListId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_LikesListId",
                table: "Photos",
                column: "LikesListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_EFGuidCollection_DisikesListId",
                table: "Photos",
                column: "DisikesListId",
                principalTable: "EFGuidCollection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_EFGuidCollection_LikesListId",
                table: "Photos",
                column: "LikesListId",
                principalTable: "EFGuidCollection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_EFGuidCollection_DisikesListId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_EFGuidCollection_LikesListId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "EFGuidCollection");

            migrationBuilder.DropIndex(
                name: "IX_Photos_DisikesListId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_LikesListId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DisikesListId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "LikesListId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "LikesCount",
                table: "Photos",
                newName: "Likes");

            migrationBuilder.RenameColumn(
                name: "DislikesCount",
                table: "Photos",
                newName: "Dislikes");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikeState = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                });
        }
    }
}
