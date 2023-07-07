using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixEntityConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BagLimitEntries_Seasons_TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_BagLimitEntries_TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_HarvestMethod_Enum",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries");

            migrationBuilder.RenameColumn(
                name: "HarvestMethod",
                table: "Activities",
                newName: "TrappedActivity_HarvestMethod");

            migrationBuilder.AddColumn<string>(
                name: "CustomWildlifeActPermit_Conditions",
                table: "Authorizations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 110, 120, 130, 200, 210, 220, 230, 300, 310)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_TrappedActivity_HarvestMethod_Enum",
                table: "Activities",
                sql: "[TrappedActivity_HarvestMethod] IN (10, 20, 30, 40)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_TrappedActivity_HarvestMethod_Enum",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CustomWildlifeActPermit_Conditions",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "TrappedActivity_HarvestMethod",
                table: "Activities",
                newName: "HarvestMethod");

            migrationBuilder.AddColumn<int>(
                name: "TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 101, 102, 103, 200, 201, 202, 300, 301)");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries",
                column: "TrappingBagLimitEntry_SeasonId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_HarvestMethod_Enum",
                table: "Activities",
                sql: "[HarvestMethod] IN (10, 20, 30, 40)");

            migrationBuilder.AddForeignKey(
                name: "FK_BagLimitEntries_Seasons_TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries",
                column: "TrappingBagLimitEntry_SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
