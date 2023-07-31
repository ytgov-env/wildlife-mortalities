using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHrbsNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HarvestActivity_HrbsNumber",
                table: "Activities",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_HarvestActivity_HrbsNumber",
                table: "Activities",
                column: "HarvestActivity_HrbsNumber",
                unique: true,
                filter: "[HarvestActivity_HrbsNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_HarvestActivity_HrbsNumber",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "HarvestActivity_HrbsNumber",
                table: "Activities");
        }
    }
}
