using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByPropertyToDraftReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateSubmitted",
                table: "DraftReports",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateLastModified",
                table: "DraftReports",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "DraftReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "DraftReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftReports_CreatedById",
                table: "DraftReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DraftReports_LastModifiedById",
                table: "DraftReports",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftReports_Users_CreatedById",
                table: "DraftReports",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftReports_Users_LastModifiedById",
                table: "DraftReports",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftReports_Users_CreatedById",
                table: "DraftReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftReports_Users_LastModifiedById",
                table: "DraftReports");

            migrationBuilder.DropIndex(
                name: "IX_DraftReports_CreatedById",
                table: "DraftReports");

            migrationBuilder.DropIndex(
                name: "IX_DraftReports_LastModifiedById",
                table: "DraftReports");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DraftReports");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "DraftReports");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "DraftReports",
                newName: "DateSubmitted");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateLastModified",
                table: "DraftReports",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
