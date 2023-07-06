using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOutfitterGuideRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OutfitterGuides_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "ReportAsAssistantGuideId",
                table: "OutfitterGuides",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReportAsChiefGuideId",
                table: "OutfitterGuides",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterGuides_ReportAsChiefGuideId",
                table: "OutfitterGuides",
                column: "ReportAsChiefGuideId",
                unique: true,
                filter: "[ReportAsChiefGuideId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OutfitterGuides_Reports_ReportAsChiefGuideId",
                table: "OutfitterGuides",
                column: "ReportAsChiefGuideId",
                principalTable: "Reports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutfitterGuides_Reports_ReportAsChiefGuideId",
                table: "OutfitterGuides");

            migrationBuilder.DropIndex(
                name: "IX_OutfitterGuides_ReportAsChiefGuideId",
                table: "OutfitterGuides");

            migrationBuilder.DropColumn(
                name: "ReportAsChiefGuideId",
                table: "OutfitterGuides");

            migrationBuilder.AddColumn<int>(
                name: "OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReportAsAssistantGuideId",
                table: "OutfitterGuides",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                unique: true,
                filter: "[OutfitterGuidedHuntReport_ChiefGuideId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_OutfitterGuides_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                principalTable: "OutfitterGuides",
                principalColumn: "Id");
        }
    }
}
