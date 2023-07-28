using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLengthToThirdAnnulusForGoat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "MountainGoatBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres",
                table: "BioSubmissions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30, 100, 110, 120, 130, 200, 210, 220, 230, 240, 300, 310, 400)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.AddColumn<int>(
                name: "MountainGoatBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres",
                table: "BioSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 110, 120, 130, 200, 210, 220, 230, 240, 300, 310, 400)");
        }
    }
}
