using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePreemptionSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PreemptionHorizontalFactor",
                table: "Profiles",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PreemptionStabilizationBufferSize",
                table: "Profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreemptionStabilizationMethod",
                table: "Profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PreemptionVerticalFactor",
                table: "Profiles",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreemptionHorizontalFactor",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PreemptionStabilizationBufferSize",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PreemptionStabilizationMethod",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PreemptionVerticalFactor",
                table: "Profiles");
        }
    }
}
