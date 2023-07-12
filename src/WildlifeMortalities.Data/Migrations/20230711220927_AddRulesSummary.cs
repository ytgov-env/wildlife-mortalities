using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRulesSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RulesSummaryId",
                table: "Violations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RulesSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulesSummaries_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationRulesSummary",
                columns: table => new
                {
                    RulesSummariesId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationRulesSummary", x => new { x.RulesSummariesId, x.AuthorizationsId });
                    table.ForeignKey(
                        name: "FK_AuthorizationRulesSummary_Authorizations_AuthorizationsId",
                        column: x => x.AuthorizationsId,
                        principalTable: "Authorizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthorizationRulesSummary_RulesSummaries_RulesSummariesId",
                        column: x => x.RulesSummariesId,
                        principalTable: "RulesSummaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Violations_RulesSummaryId",
                table: "Violations",
                column: "RulesSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationRulesSummary_AuthorizationsId",
                table: "AuthorizationRulesSummary",
                column: "AuthorizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesSummaries_ReportId",
                table: "RulesSummaries",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_RulesSummaries_RulesSummaryId",
                table: "Violations",
                column: "RulesSummaryId",
                principalTable: "RulesSummaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Violations_RulesSummaries_RulesSummaryId",
                table: "Violations");

            migrationBuilder.DropTable(
                name: "AuthorizationRulesSummary");

            migrationBuilder.DropTable(
                name: "RulesSummaries");

            migrationBuilder.DropIndex(
                name: "IX_Violations_RulesSummaryId",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "RulesSummaryId",
                table: "Violations");
        }
    }
}
