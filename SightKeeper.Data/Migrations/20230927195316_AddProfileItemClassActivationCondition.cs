using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileItemClassActivationCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivationCondition",
                table: "ProfileItemClasses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationCondition",
                table: "ProfileItemClasses");
        }
    }
}
