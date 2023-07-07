using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEyeSocketBooleanForSheep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.AddColumn<bool>(
                name: "ThinhornSheepBioSubmission_IsBothEyeSocketsComplete",
                table: "BioSubmissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 100, 101, 102, 103, 200, 201, 202, 300, 301)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "ThinhornSheepBioSubmission_IsBothEyeSocketsComplete",
                table: "BioSubmissions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30, 40, 50, 60)");
        }
    }
}
