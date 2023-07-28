using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSheepDnaSampleProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_IsDnaSampleTaken",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_IsDnaSampleExtracted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_IsDnaSampleExtracted",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_IsDnaSampleTaken");
        }
    }
}
