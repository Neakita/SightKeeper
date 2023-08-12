using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparateModelWeightsIntoInternalTrainedAndPreTrained : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_ModelConfigs_ConfigId",
                table: "Models");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelWeights_ModelConfigs_ConfigId",
                table: "ModelWeights");

            migrationBuilder.DropTable(
                name: "WeightsAssets");

            migrationBuilder.DropIndex(
                name: "IX_Models_ConfigId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ConfigId",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ModelWeights",
                newName: "TrainedDate");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ModelWeights",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Batch",
                table: "ModelWeights",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<float>(
                name: "Accuracy",
                table: "ModelWeights",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ModelWeights",
                type: "TEXT",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Models",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "AssetInternalTrainedModelWeights",
                columns: table => new
                {
                    AssetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    InternalTrainedModelWeightsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetInternalTrainedModelWeights", x => new { x.AssetsId, x.InternalTrainedModelWeightsId });
                    table.ForeignKey(
                        name: "FK_AssetInternalTrainedModelWeights_Assets_AssetsId",
                        column: x => x.AssetsId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetInternalTrainedModelWeights_ModelWeights_InternalTrainedModelWeightsId",
                        column: x => x.InternalTrainedModelWeightsId,
                        principalTable: "ModelWeights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetInternalTrainedModelWeights_InternalTrainedModelWeightsId",
                table: "AssetInternalTrainedModelWeights",
                column: "InternalTrainedModelWeightsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelWeights_ModelConfigs_ConfigId",
                table: "ModelWeights",
                column: "ConfigId",
                principalTable: "ModelConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelWeights_ModelConfigs_ConfigId",
                table: "ModelWeights");

            migrationBuilder.DropTable(
                name: "AssetInternalTrainedModelWeights");

            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "ModelWeights");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ModelWeights");

            migrationBuilder.RenameColumn(
                name: "TrainedDate",
                table: "ModelWeights",
                newName: "Date");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ModelWeights",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Batch",
                table: "ModelWeights",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Models",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ConfigId",
                table: "Models",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DetectorItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Models_ConfigId",
                table: "Models",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightsAssets_WeightsId",
                table: "WeightsAssets",
                column: "WeightsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_ModelConfigs_ConfigId",
                table: "Models",
                column: "ConfigId",
                principalTable: "ModelConfigs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelWeights_ModelConfigs_ConfigId",
                table: "ModelWeights",
                column: "ConfigId",
                principalTable: "ModelConfigs",
                principalColumn: "Id");
        }
    }
}
