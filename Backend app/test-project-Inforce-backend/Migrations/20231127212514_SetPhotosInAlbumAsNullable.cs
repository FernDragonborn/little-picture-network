using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_project_Inforce_backend.Migrations
{
    /// <inheritdoc />
    public partial class SetPhotosInAlbumAsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums");

            migrationBuilder.AlterColumn<int>(
                name: "PhotosId",
                table: "Albums",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums",
                column: "PhotosId",
                principalTable: "EFGuidCollection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums");

            migrationBuilder.AlterColumn<int>(
                name: "PhotosId",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_EFGuidCollection_PhotosId",
                table: "Albums",
                column: "PhotosId",
                principalTable: "EFGuidCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
