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
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Resolution_Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Resolution_Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
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
                    MaxQuantity = table.Column<ushort>(type: "INTEGER", nullable: true),
                    HasAnyScreenshots = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenshotsLibraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemClasses_Models_DataSetId",
                        column: x => x.DataSetId,
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
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelScreenshotsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelScreenshotsLibraries_Models_DataSetId",
                        column: x => x.DataSetId,
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
                        name: "FK_Profiles_DetectorModels_DetectorDataSetId",
                        column: x => x.DetectorDataSetId,
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
                name: "Weights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    TrainedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 34, nullable: false),
                    Batch = table.Column<int>(type: "INTEGER", nullable: true),
                    BoundingLoss = table.Column<float>(type: "REAL", nullable: true),
                    ClassificationLoss = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weights_ModelConfigs_ModelId",
                        column: x => x.ModelId,
                        principalTable: "ModelConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Weights_ModelWeightsLibraries_LibraryId",
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
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false)
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
                        name: "FK_DetectorAssets_DetectorModels_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DetectorModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetInternalTrainedWeights",
                columns: table => new
                {
                    AssetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    InternalTrainedWeightsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetInternalTrainedWeights", x => new { x.AssetsId, x.InternalTrainedWeightsId });
                    table.ForeignKey(
                        name: "FK_AssetInternalTrainedWeights_Assets_AssetsId",
                        column: x => x.AssetsId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetInternalTrainedWeights_Weights_InternalTrainedWeightsId",
                        column: x => x.InternalTrainedWeightsId,
                        principalTable: "Weights",
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
                name: "IX_AssetInternalTrainedWeights_InternalTrainedWeightsId",
                table: "AssetInternalTrainedWeights",
                column: "InternalTrainedWeightsId");

            migrationBuilder.CreateIndex(
                name: "IX_DetectorAssets_DataSetId",
                table: "DetectorAssets",
                column: "DataSetId");

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
                name: "IX_Models_GameId",
                table: "Models",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelScreenshotsLibraries_DataSetId",
                table: "ModelScreenshotsLibraries",
                column: "DataSetId",
                unique: true);

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
                name: "IX_Weights_LibraryId",
                table: "Weights",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Weights_ModelId",
                table: "Weights",
                column: "ModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetInternalTrainedWeights");

            migrationBuilder.DropTable(
                name: "DetectorItems");

            migrationBuilder.DropTable(
                name: "ModelScreenshotsLibraries");

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
                name: "ModelConfigs");

            migrationBuilder.DropTable(
                name: "ModelWeightsLibraries");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "DetectorModels");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "ScreenshotsLibraries");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
