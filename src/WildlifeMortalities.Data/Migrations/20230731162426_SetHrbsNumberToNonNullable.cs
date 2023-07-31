using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetHrbsNumberToNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Activities_HarvestActivity_HrbsNumber",
                table: "Activities");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30, 40, 100, 110, 120, 130, 200, 210, 220, 230, 240, 300, 310)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30, 100, 110, 120, 130, 200, 210, 220, 230, 240, 300, 310, 400)");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_HarvestActivity_HrbsNumber",
                table: "Activities",
                column: "HarvestActivity_HrbsNumber",
                unique: true,
                filter: "[HarvestActivity_HrbsNumber] IS NOT NULL");
        }
    }
}
