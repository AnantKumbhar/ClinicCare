using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicCare.Migrations
{
    /// <inheritdoc />
    public partial class LinkExpenseToMedicinePurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicinePurchaseId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_MedicinePurchaseId",
                table: "Expenses",
                column: "MedicinePurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_MedicinePurchases_MedicinePurchaseId",
                table: "Expenses",
                column: "MedicinePurchaseId",
                principalTable: "MedicinePurchases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_MedicinePurchases_MedicinePurchaseId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_MedicinePurchaseId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "MedicinePurchaseId",
                table: "Expenses");
        }
    }
}
