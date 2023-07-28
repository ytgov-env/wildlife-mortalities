using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBoolPropertiesForBearToothExtracted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AmericanBlackBearBioSubmission_IsToothExtracted",
                table: "BioSubmissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GrizzlyBearBioSubmission_IsToothExtracted",
                table: "BioSubmissions",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmericanBlackBearBioSubmission_IsToothExtracted",
                table: "BioSubmissions");

            migrationBuilder.DropColumn(
                name: "GrizzlyBearBioSubmission_IsToothExtracted",
                table: "BioSubmissions");
        }
    }
}
