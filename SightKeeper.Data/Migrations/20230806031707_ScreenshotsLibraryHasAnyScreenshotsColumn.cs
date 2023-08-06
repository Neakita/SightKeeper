using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScreenshotsLibraryHasAnyScreenshotsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAnyScreenshots",
                table: "ScreenshotsLibraries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.Sql(@"UPDATE ScreenshotsLibraries
SET HasAnyScreenshots = (SELECT COUNT(*) > 0 FROM Screenshots WHERE LibraryId = ScreenshotsLibraries.Id);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAnyScreenshots",
                table: "ScreenshotsLibraries");
        }
    }
}
