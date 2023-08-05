using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class EncapsulateContentFromScreenshotToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreenshotImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScreenshotId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenshotImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenshotImages_Images_Id",
                        column: x => x.Id,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreenshotImages_Screenshots_ScreenshotId",
                        column: x => x.ScreenshotId,
                        principalTable: "Screenshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreenshotImages_ScreenshotId",
                table: "ScreenshotImages",
                column: "ScreenshotId",
                unique: true);

            migrationBuilder.Sql(@"CREATE TABLE temp_ScreenshotImages
(
    Id           INTEGER NOT NULL
        CONSTRAINT PK_ScreenshotImages PRIMARY KEY,
    ScreenshotId INTEGER NOT NULL
);

INSERT INTO temp_ScreenshotImages (Id, ScreenshotId)
SELECT row_number() over (), Id
FROM Screenshots;

INSERT INTO Images (Id, Content)
SELECT image.Id, screenshot.Content
FROM temp_ScreenshotImages AS image
         JOIN Screenshots screenshot ON screenshot.Id = image.ScreenshotId;

INSERT INTO ScreenshotImages (Id, ScreenshotId)
SELECT *
FROM temp_ScreenshotImages;

DROP TABLE temp_ScreenshotImages;");
            
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Screenshots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Screenshots",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.Sql(@"UPDATE Screenshots
SET Content = (
    SELECT Images.Content
    FROM Images
             JOIN ScreenshotImages ON ScreenshotImages.Id = Images.Id
    WHERE ScreenshotImages.ScreenshotId = Screenshots.Id
);");
            
            migrationBuilder.DropTable(
                name: "ScreenshotImages");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
