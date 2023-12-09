using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightsItemClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_WeightsItemClasses_ItemClassId",
                table: "WeightsItemClasses",
                column: "ItemClassId");

            migrationBuilder.Sql(
                """
                INSERT INTO WeightsItemClasses
                SELECT Weights.Id as WeightsId, ItemClasses.Id as ItemClassId
                FROM Weights
                JOIN ItemClasses ON ItemClasses.DataSetId = Weights.LibraryId;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightsItemClasses");
        }
    }
}
