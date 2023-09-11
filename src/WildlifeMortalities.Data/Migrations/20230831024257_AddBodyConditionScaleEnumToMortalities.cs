using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBodyConditionScaleEnumToMortalities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BodyConditionScale",
                table: "Mortalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // We are using a check constraint to prevent default values from being inserted
            // (zero is not a value in the enum, and therefore would only be the result of a bug),
            // but we must set all existing values to -1 (not specified) before we can add the check constraint
            migrationBuilder.Sql("update mortalities set BodyConditionScale = -1 where BodyConditionScale = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities",
                sql: "[BodyConditionScale] IN (10, 20, 30, 40, 50, 60, -1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities");

            migrationBuilder.DropColumn(
                name: "BodyConditionScale",
                table: "Mortalities");
        }
    }
}
