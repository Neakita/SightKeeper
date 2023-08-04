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
                name: "ModelConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ModelType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreenshotsLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxQuantity = table.Column<ushort>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenshotsLibraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true),
                    ConfigId = table.Column<int>(type: "INTEGER", nullable: true),
                    Resolution_Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Resolution_Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Models_ModelConfigs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ModelConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "DetectorModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetectorModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetectorModels_Models_Id",
                        column: x => x.Id,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemClasses_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelScreenshotsLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelScreenshotsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelScreenshotsLibraries_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelScreenshotsLibraries_ScreenshotsLibraries_Id",
                        column: x => x.Id,
                        principalTable: "ScreenshotsLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelWeightsLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelWeightsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelWeightsLibraries_Models_Id",
                        column: x => x.Id,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Screenshots_Id",
                        column: x => x.Id,
                        principalTable: "Screenshots",
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
                    DetectorModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_DetectorModels_DetectorModelId",
                        column: x => x.DetectorModelId,
                        principalTable: "DetectorModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModelWeights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Batch = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ConfigId = table.Column<int>(type: "INTEGER", nullable: true),
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelWeights_ModelConfigs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ModelConfigs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelWeights_ModelWeightsLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "ModelWeightsLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetectorAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false)
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
                        name: "FK_DetectorAssets_DetectorModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "DetectorModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightsAssets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightsAssets", x => new { x.AssetId, x.WeightsId });
                    table.ForeignKey(
                        name: "FK_WeightsAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightsAssets_ModelWeights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "ModelWeights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetectorItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    BoundingBox_X1 = table.Column<double>(type: "REAL", nullable: false),
                    BoundingBox_Y1 = table.Column<double>(type: "REAL", nullable: false),
                    BoundingBox_X2 = table.Column<double>(type: "REAL", nullable: false),
                    BoundingBox_Y2 = table.Column<double>(type: "REAL", nullable: false)
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
                name: "IX_DetectorAssets_ModelId",
                table: "DetectorAssets",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_AssetId",
                table: "DetectorItems",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_DetectorItems_ItemClassId",
                table: "DetectorItems",
                column: "ItemClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemClasses_ModelId",
                table: "ItemClasses",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ConfigId",
                table: "Models",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_GameId",
                table: "Models",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelScreenshotsLibraries_ModelId",
                table: "ModelScreenshotsLibraries",
                column: "ModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelWeights_ConfigId",
                table: "ModelWeights",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelWeights_LibraryId",
                table: "ModelWeights",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_DetectorModelId",
                table: "Profiles",
                column: "DetectorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_GameId",
                table: "Profiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_LibraryId",
                table: "Screenshots",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightsAssets_WeightsId",
                table: "WeightsAssets",
                column: "WeightsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetectorItems");

            migrationBuilder.DropTable(
                name: "ModelScreenshotsLibraries");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "WeightsAssets");

            migrationBuilder.DropTable(
                name: "DetectorAssets");

            migrationBuilder.DropTable(
                name: "ItemClasses");

            migrationBuilder.DropTable(
                name: "ModelWeights");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "DetectorModels");

            migrationBuilder.DropTable(
                name: "ModelWeightsLibraries");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "ScreenshotsLibraries");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "ModelConfigs");
        }
    }
}
