using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeclinedToAnswerOptionToBCS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities",
                sql: "[BodyConditionScale] IN (10, 20, 30, 40, 50, 60, 70, -1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_BodyConditionScale_Enum",
                table: "Mortalities",
                sql: "[BodyConditionScale] IN (10, 20, 30, 40, 50, 60, -1)");
        }
    }
}
