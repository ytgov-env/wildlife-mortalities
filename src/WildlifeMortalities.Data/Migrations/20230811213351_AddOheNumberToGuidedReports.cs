using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOheNumberToGuidedReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutfitterGuidedHuntReport_OheNumber",
                table: "Reports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialGuidedHuntReport_OheNumber",
                table: "Reports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutfitterGuidedHuntReport_OheNumber",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SpecialGuidedHuntReport_OheNumber",
                table: "Reports");
        }
    }
}
