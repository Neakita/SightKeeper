using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

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
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSets_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemClasses_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true),
                    DetectorDataSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_DataSets_DetectorDataSetId",
                        column: x => x.DetectorDataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScreenshotsLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxQuantity = table.Column<ushort>(type: "INTEGER", nullable: true),
                    HasAnyScreenshots = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenshotsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenshotsLibraries_DataSets_Id",
                        column: x => x.Id,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightsLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightsLibraries_DataSets_Id",
                        column: x => x.Id,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScreenshotsLibraryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshots_ScreenshotsLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "ScreenshotsLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Screenshots_ScreenshotsLibraries_ScreenshotsLibraryId",
                        column: x => x.ScreenshotsLibraryId,
                        principalTable: "ScreenshotsLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Epoch = table.Column<uint>(type: "INTEGER", nullable: false),
                    Loss = table.Column<float>(type: "REAL", nullable: false),
                    WeightsLibraryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weights_WeightsLibraries_WeightsLibraryId",
                        column: x => x.WeightsLibraryId,
                        principalTable: "WeightsLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    DataSetId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Usage = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_DataSets_DataSetId1",
                        column: x => x.DataSetId1,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_Screenshots_Id",
                        column: x => x.Id,
                        principalTable: "Screenshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "DetectorAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    DataSetDetectorAssetId = table.Column<int>(name: "DataSet<DetectorAsset>Id", type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetectorAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetectorAssets_Assets_Id",
                        column: x => x.Id,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetectorAssets_DataSets_DataSet<DetectorAsset>Id",
                        column: x => x.DataSetDetectorAssetId,
                        principalTable: "DataSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetectorItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    Bounding_Left = table.Column<double>(type: "REAL", nullable: false),
                    Bounding_Top = table.Column<double>(type: "REAL", nullable: false),
                    Bounding_Right = table.Column<double>(type: "REAL", nullable: false),
                    Bounding_Bottom = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetectorItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetectorItems_DetectorAssets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "DetectorAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetectorItems_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DataSetId1",
                table: "Assets",
                column: "DataSetId1");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_GameId",
                table: "DataSets",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_Name",
                table: "DataSets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetectorAssets_DataSet<DetectorAsset>Id",
                table: "DetectorAssets",
                column: "DataSet<DetectorAsset>Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_AssetId",
                table: "DetectorItems",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_ItemClassId",
                table: "DetectorItems",
                column: "ItemClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemClasses_DataSetId",
                table: "ItemClasses",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_DetectorDataSetId",
                table: "Profiles",
                column: "DetectorDataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_GameId",
                table: "Profiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenshotImages_ScreenshotId",
                table: "ScreenshotImages",
                column: "ScreenshotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_LibraryId",
                table: "Screenshots",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_ScreenshotsLibraryId",
                table: "Screenshots",
                column: "ScreenshotsLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Weights_WeightsLibraryId",
                table: "Weights",
                column: "WeightsLibraryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetectorItems");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ScreenshotImages");

            migrationBuilder.DropTable(
                name: "Weights");

            migrationBuilder.DropTable(
                name: "DetectorAssets");

            migrationBuilder.DropTable(
                name: "ItemClasses");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "WeightsLibraries");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "ScreenshotsLibraries");

            migrationBuilder.DropTable(
                name: "DataSets");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
