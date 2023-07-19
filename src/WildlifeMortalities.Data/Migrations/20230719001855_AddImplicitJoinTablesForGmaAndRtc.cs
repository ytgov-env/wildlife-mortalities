using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImplicitJoinTablesForGmaAndRtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameManagementAreas_BagLimitEntries_HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredTrappingConcessions_BagLimitEntries_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredTrappingConcessions_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropIndex(
                name: "IX_GameManagementAreas_HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.DropColumn(
                name: "TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropColumn(
                name: "HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.CreateTable(
                name: "GameManagementAreaHuntingBagLimitEntry",
                columns: table => new
                {
                    AreasId = table.Column<int>(type: "int", nullable: false),
                    HuntingBagLimitEntriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameManagementAreaHuntingBagLimitEntry", x => new { x.AreasId, x.HuntingBagLimitEntriesId });
                    table.ForeignKey(
                        name: "FK_GameManagementAreaHuntingBagLimitEntry_BagLimitEntries_HuntingBagLimitEntriesId",
                        column: x => x.HuntingBagLimitEntriesId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameManagementAreaHuntingBagLimitEntry_GameManagementAreas_AreasId",
                        column: x => x.AreasId,
                        principalTable: "GameManagementAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredTrappingConcessionTrappingBagLimitEntry",
                columns: table => new
                {
                    ConcessionsId = table.Column<int>(type: "int", nullable: false),
                    TrappingBagLimitEntriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredTrappingConcessionTrappingBagLimitEntry", x => new { x.ConcessionsId, x.TrappingBagLimitEntriesId });
                    table.ForeignKey(
                        name: "FK_RegisteredTrappingConcessionTrappingBagLimitEntry_BagLimitEntries_TrappingBagLimitEntriesId",
                        column: x => x.TrappingBagLimitEntriesId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredTrappingConcessionTrappingBagLimitEntry_RegisteredTrappingConcessions_ConcessionsId",
                        column: x => x.ConcessionsId,
                        principalTable: "RegisteredTrappingConcessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameManagementAreaHuntingBagLimitEntry_HuntingBagLimitEntriesId",
                table: "GameManagementAreaHuntingBagLimitEntry",
                column: "HuntingBagLimitEntriesId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredTrappingConcessionTrappingBagLimitEntry_TrappingBagLimitEntriesId",
                table: "RegisteredTrappingConcessionTrappingBagLimitEntry",
                column: "TrappingBagLimitEntriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameManagementAreaHuntingBagLimitEntry");

            migrationBuilder.DropTable(
                name: "RegisteredTrappingConcessionTrappingBagLimitEntry");

            migrationBuilder.AddColumn<int>(
                name: "TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredTrappingConcessions_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                column: "TrappingBagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_GameManagementAreas_HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                column: "HuntingBagLimitEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameManagementAreas_BagLimitEntries_HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                column: "HuntingBagLimitEntryId",
                principalTable: "BagLimitEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredTrappingConcessions_BagLimitEntries_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                column: "TrappingBagLimitEntryId",
                principalTable: "BagLimitEntries",
                principalColumn: "Id");
        }
    }
}
