using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicCare.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseSystemGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemGenerated",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemGenerated",
                table: "Expenses");
        }
    }
}
