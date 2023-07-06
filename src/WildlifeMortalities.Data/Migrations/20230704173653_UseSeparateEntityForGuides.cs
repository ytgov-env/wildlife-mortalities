using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class UseSeparateEntityForGuides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "AssistantGuideOutfitterGuidedHuntReport");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_BagEntries_PersonId",
                table: "BagEntries");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Violations");

            migrationBuilder.AddColumn<int>(
                name: "HarvestMethod",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OutfitterGuides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReportAsAssistantGuideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitterGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutfitterGuides_Reports_ReportAsAssistantGuideId",
                        column: x => x.ReportAsAssistantGuideId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30, 40, 50, 60)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                unique: true,
                filter: "[OutfitterGuidedHuntReport_ChiefGuideId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_Species_Sex_PeriodStart_PeriodEnd",
                table: "BagLimitEntries",
                columns: new[] { "Species", "Sex", "PeriodStart", "PeriodEnd" },
                unique: true,
                filter: "[Sex] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BagEntries_PersonId_BagLimitEntryId",
                table: "BagEntries",
                columns: new[] { "PersonId", "BagLimitEntryId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_HarvestMethod_Enum",
                table: "Activities",
                sql: "[HarvestMethod] IN (10, 20, 30, 40)");

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterGuides_ReportAsAssistantGuideId",
                table: "OutfitterGuides",
                column: "ReportAsAssistantGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_OutfitterGuides_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                principalTable: "OutfitterGuides",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OutfitterGuides_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "OutfitterGuides");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_BagLimitEntries_Species_Sex_PeriodStart_PeriodEnd",
                table: "BagLimitEntries");

            migrationBuilder.DropIndex(
                name: "IX_BagEntries_PersonId_BagLimitEntryId",
                table: "BagEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_HarvestMethod_Enum",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "HarvestMethod",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Violations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AssistantGuideOutfitterGuidedHuntReport",
                columns: table => new
                {
                    AssistantGuidesId = table.Column<int>(type: "int", nullable: false),
                    OutfitterGuidedHuntReportsAsAssistantGuideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantGuideOutfitterGuidedHuntReport", x => new { x.AssistantGuidesId, x.OutfitterGuidedHuntReportsAsAssistantGuideId });
                    table.ForeignKey(
                        name: "FK_AssistantGuideOutfitterGuidedHuntReport_People_AssistantGuidesId",
                        column: x => x.AssistantGuidesId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssistantGuideOutfitterGuidedHuntReport_Reports_OutfitterGuidedHuntReportsAsAssistantGuideId",
                        column: x => x.OutfitterGuidedHuntReportsAsAssistantGuideId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_BagEntries_PersonId",
                table: "BagEntries",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantGuideOutfitterGuidedHuntReport_OutfitterGuidedHuntReportsAsAssistantGuideId",
                table: "AssistantGuideOutfitterGuidedHuntReport",
                column: "OutfitterGuidedHuntReportsAsAssistantGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
