using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastModifiedByToBioSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "BioSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_LastModifiedById",
                table: "BioSubmissions",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_BioSubmissions_Users_LastModifiedById",
                table: "BioSubmissions",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BioSubmissions_Users_LastModifiedById",
                table: "BioSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_BioSubmissions_LastModifiedById",
                table: "BioSubmissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "BioSubmissions");
        }
    }
}
