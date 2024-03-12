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
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessName = table.Column<string>(type: "TEXT", nullable: false),
                    ExecutablePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true),
                    Resolution = table.Column<ushort>(type: "INTEGER", nullable: false)
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
                name: "AssetsLibraries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsLibraries_DataSets_Id",
                        column: x => x.Id,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<uint>(type: "INTEGER", nullable: false),
                    DataSetId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "ScreenshotsLibraries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    MaxQuantity = table.Column<ushort>(type: "INTEGER", nullable: true)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshots_ScreenshotsLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "ScreenshotsLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Weights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<long>(type: "INTEGER", nullable: true),
                    Epoch = table.Column<uint>(type: "INTEGER", nullable: false),
                    BoundingLoss = table.Column<float>(type: "REAL", nullable: false),
                    ClassificationLoss = table.Column<float>(type: "REAL", nullable: false),
                    DeformationLoss = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weights_WeightsLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "WeightsLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Usage = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetsLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "AssetsLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assets_Screenshots_Id",
                        column: x => x.Id,
                        principalTable: "Screenshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Screenshots_Id",
                        column: x => x.Id,
                        principalTable: "Screenshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DetectionThreshold = table.Column<float>(type: "REAL", nullable: false),
                    MouseSensitivity = table.Column<float>(type: "REAL", nullable: false),
                    PostProcessDelay = table.Column<ushort>(type: "INTEGER", nullable: false),
                    WeightsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Weights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "Weights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightsData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    WeightsId = table.Column<long>(type: "INTEGER", nullable: false),
                    Type = table.Column<byte>(type: "INTEGER", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightsData_Weights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "Weights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightsItemClasses",
                columns: table => new
                {
                    WeightsId = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemClassId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightsItemClasses", x => new { x.WeightsId, x.ItemClassId });
                    table.ForeignKey(
                        name: "FK_WeightsItemClasses_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightsItemClasses_Weights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "Weights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetectorItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemClassId = table.Column<long>(type: "INTEGER", nullable: false),
                    AssetId = table.Column<long>(type: "INTEGER", nullable: false),
                    BoundingXPosition = table.Column<double>(type: "REAL", nullable: false),
                    BoundingYPosition = table.Column<double>(type: "REAL", nullable: false),
                    BoundingXSize = table.Column<double>(type: "REAL", nullable: false),
                    BoundingYSize = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetectorItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetectorItems_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetectorItems_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreemptionSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    PreemptionStabilizationBufferSize = table.Column<byte>(type: "INTEGER", nullable: true),
                    PreemptionStabilizationMethod = table.Column<int>(type: "INTEGER", nullable: true),
                    PreemptionHorizontalFactor = table.Column<float>(type: "REAL", nullable: false),
                    PreemptionVerticalFactor = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreemptionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreemptionSettings_Profiles_Id",
                        column: x => x.Id,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileItemClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemClassId = table.Column<long>(type: "INTEGER", nullable: false),
                    Index = table.Column<byte>(type: "INTEGER", nullable: false),
                    ActivationCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileItemClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileItemClasses_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileItemClasses_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LibraryId",
                table: "Assets",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_GameId",
                table: "DataSets",
                column: "GameId");

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
                name: "IX_ProfileItemClasses_ItemClassId",
                table: "ProfileItemClasses",
                column: "ItemClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileItemClasses_ProfileId_Index",
                table: "ProfileItemClasses",
                columns: new[] { "ProfileId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileItemClasses_ProfileId_ItemClassId",
                table: "ProfileItemClasses",
                columns: new[] { "ProfileId", "ItemClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Name",
                table: "Profiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_WeightsId",
                table: "Profiles",
                column: "WeightsId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_LibraryId",
                table: "Screenshots",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Weights_LibraryId",
                table: "Weights",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightsData_Type_WeightsId",
                table: "WeightsData",
                columns: new[] { "Type", "WeightsId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeightsData_WeightsId",
                table: "WeightsData",
                column: "WeightsId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightsItemClasses_ItemClassId",
                table: "WeightsItemClasses",
                column: "ItemClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetectorItems");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PreemptionSettings");

            migrationBuilder.DropTable(
                name: "ProfileItemClasses");

            migrationBuilder.DropTable(
                name: "WeightsData");

            migrationBuilder.DropTable(
                name: "WeightsItemClasses");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ItemClasses");

            migrationBuilder.DropTable(
                name: "AssetsLibraries");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Weights");

            migrationBuilder.DropTable(
                name: "ScreenshotsLibraries");

            migrationBuilder.DropTable(
                name: "WeightsLibraries");

            migrationBuilder.DropTable(
                name: "DataSets");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
