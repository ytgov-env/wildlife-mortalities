using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityyId",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "HuntedActivityyId",
                table: "Authorizations",
                newName: "HuntedActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_HuntedActivityyId",
                table: "Authorizations",
                newName: "IX_Authorizations_HuntedActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityId",
                table: "Authorizations",
                column: "HuntedActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityId",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "HuntedActivityId",
                table: "Authorizations",
                newName: "HuntedActivityyId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_HuntedActivityId",
                table: "Authorizations",
                newName: "IX_Authorizations_HuntedActivityyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityyId",
                table: "Authorizations",
                column: "HuntedActivityyId",
                principalTable: "Activities",
                principalColumn: "Id");
        }
    }
}
