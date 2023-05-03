using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class SpecifyDetectorItemScreenshotId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetectorItems_DetectorScreenshots_DetectorScreenshotId",
                table: "DetectorItems");

            migrationBuilder.DropIndex(
                name: "IX_DetectorItems_DetectorScreenshotId",
                table: "DetectorItems");

            migrationBuilder.DropColumn(
                name: "DetectorScreenshotId",
                table: "DetectorItems");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ScreenshotId",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_ScreenshotId",
                table: "DetectorItems",
                column: "ScreenshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetectorItems_DetectorScreenshots_ScreenshotId",
                table: "DetectorItems",
                column: "ScreenshotId",
                principalTable: "DetectorScreenshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetectorItems_DetectorScreenshots_ScreenshotId",
                table: "DetectorItems");

            migrationBuilder.DropIndex(
                name: "IX_DetectorItems_ScreenshotId",
                table: "DetectorItems");

            migrationBuilder.DropColumn(
                name: "ScreenshotId",
                table: "DetectorItems");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "DetectorScreenshotId",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_DetectorScreenshotId",
                table: "DetectorItems",
                column: "DetectorScreenshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetectorItems_DetectorScreenshots_DetectorScreenshotId",
                table: "DetectorItems",
                column: "DetectorScreenshotId",
                principalTable: "DetectorScreenshots",
                principalColumn: "Id");
        }
    }
}
