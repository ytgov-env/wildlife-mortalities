using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSelfReferencingRelationshipForBagLimitEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BagLimitEntries_BagLimitEntries_BagLimitEntryId",
                table: "BagLimitEntries");

            migrationBuilder.DropIndex(
                name: "IX_BagLimitEntries_BagLimitEntryId",
                table: "BagLimitEntries");

            migrationBuilder.DropIndex(
                name: "IX_BagLimitEntries_Species_Sex_PeriodStart_PeriodEnd",
                table: "BagLimitEntries");

            migrationBuilder.DropColumn(
                name: "BagLimitEntryId",
                table: "BagLimitEntries");

            migrationBuilder.CreateTable(
                name: "BagLimitEntryBagLimitEntry",
                columns: table => new
                {
                    BagLimitEntryId = table.Column<int>(type: "int", nullable: false),
                    MaxValuePerPersonSharedWithId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagLimitEntryBagLimitEntry", x => new { x.BagLimitEntryId, x.MaxValuePerPersonSharedWithId });
                    table.ForeignKey(
                        name: "FK_BagLimitEntryBagLimitEntry_BagLimitEntries_BagLimitEntryId",
                        column: x => x.BagLimitEntryId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BagLimitEntryBagLimitEntry_BagLimitEntries_MaxValuePerPersonSharedWithId",
                        column: x => x.MaxValuePerPersonSharedWithId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntryBagLimitEntry_MaxValuePerPersonSharedWithId",
                table: "BagLimitEntryBagLimitEntry",
                column: "MaxValuePerPersonSharedWithId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BagLimitEntryBagLimitEntry");

            migrationBuilder.AddColumn<int>(
                name: "BagLimitEntryId",
                table: "BagLimitEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_BagLimitEntryId",
                table: "BagLimitEntries",
                column: "BagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_Species_Sex_PeriodStart_PeriodEnd",
                table: "BagLimitEntries",
                columns: new[] { "Species", "Sex", "PeriodStart", "PeriodEnd" },
                unique: true,
                filter: "[Sex] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BagLimitEntries_BagLimitEntries_BagLimitEntryId",
                table: "BagLimitEntries",
                column: "BagLimitEntryId",
                principalTable: "BagLimitEntries",
                principalColumn: "Id");
        }
    }
}
