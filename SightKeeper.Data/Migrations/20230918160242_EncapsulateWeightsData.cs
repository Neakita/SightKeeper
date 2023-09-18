using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class EncapsulateWeightsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeightsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ONNXData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeightsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONNXData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ONNXData_WeightsData_Id",
                        column: x => x.Id,
                        principalTable: "WeightsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ONNXData_Weights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "Weights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PTData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeightsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PTData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PTData_WeightsData_Id",
                        column: x => x.Id,
                        principalTable: "WeightsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PTData_Weights_WeightsId",
                        column: x => x.WeightsId,
                        principalTable: "Weights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ONNXData_WeightsId",
                table: "ONNXData",
                column: "WeightsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PTData_WeightsId",
                table: "PTData",
                column: "WeightsId",
                unique: true);

            migrationBuilder.Sql(
                @"
INSERT INTO WeightsData (Id, Content)
SELECT ROW_NUMBER() OVER() * 2 - 1, Weights.PTData
FROM Weights;

INSERT INTO WeightsData (Id, Content)
SELECT ROW_NUMBER() OVER() * 2, Weights.ONNXData
FROM Weights;

INSERT INTO PTData (Id, WeightsId)
SELECT ROW_NUMBER() OVER() * 2 - 1, Weights.Id
FROM Weights;

INSERT INTO ONNXData (Id, WeightsId)
SELECT ROW_NUMBER() OVER() * 2, Weights.Id
FROM Weights;");
            
            migrationBuilder.DropColumn(
                name: "ONNXData",
                table: "Weights");

            migrationBuilder.DropColumn(
                name: "PTData",
                table: "Weights");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ONNXData");

            migrationBuilder.DropTable(
                name: "PTData");

            migrationBuilder.DropTable(
                name: "WeightsData");

            migrationBuilder.AddColumn<byte[]>(
                name: "ONNXData",
                table: "Weights",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PTData",
                table: "Weights",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
