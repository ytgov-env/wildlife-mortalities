using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitCaribouHerdIntoTwoProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_Herd_Enum",
                table: "Mortalities");

            migrationBuilder.DropColumn(
                name: "Herds",
                table: "BagLimitEntries");

            migrationBuilder.RenameColumn(
                name: "CaribouMortality_Herd",
                table: "Mortalities",
                newName: "CaribouMortality_LegalHerd");

            migrationBuilder.AddColumn<int>(
                name: "CaribouMortality_ActualHerd",
                table: "Mortalities",
                type: "int",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 110, 120, 130, 200, 210, 220, 230, 240, 300, 310, 400)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_ActualHerd_Enum",
                table: "Mortalities",
                sql: "[CaribouMortality_ActualHerd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, -1)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_LegalHerd_Enum",
                table: "Mortalities",
                sql: "[CaribouMortality_LegalHerd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, -1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_ActualHerd_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_LegalHerd_Enum",
                table: "Mortalities");

            migrationBuilder.DropColumn(
                name: "CaribouMortality_ActualHerd",
                table: "Mortalities");

            migrationBuilder.RenameColumn(
                name: "CaribouMortality_LegalHerd",
                table: "Mortalities",
                newName: "CaribouMortality_Herd");

            migrationBuilder.AddColumn<string>(
                name: "Herds",
                table: "BagLimitEntries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 110, 120, 130, 200, 210, 220, 230, 300, 310, 400)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_Herd_Enum",
                table: "Mortalities",
                sql: "[CaribouMortality_Herd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300)");
        }
    }
}
