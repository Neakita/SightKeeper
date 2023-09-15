﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DetectionThreshold = table.Column<float>(type: "REAL", nullable: false),
                    MouseSensitivity = table.Column<float>(type: "REAL", nullable: false),
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileItemClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true)
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
                        principalColumn: "Id");
                });

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
                name: "IX_Profiles_DataSetId",
                table: "Profiles",
                column: "DataSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileItemClasses");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
