using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicCare.Migrations
{
    /// <inheritdoc />
    public partial class LinkDiseaseCategoryToVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disease",
                table: "PatientVisits");

            migrationBuilder.AddColumn<int>(
                name: "DiseaseCategoryId",
                table: "PatientVisits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisits_DiseaseCategoryId",
                table: "PatientVisits",
                column: "DiseaseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisits_DiseaseCategories_DiseaseCategoryId",
                table: "PatientVisits",
                column: "DiseaseCategoryId",
                principalTable: "DiseaseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisits_DiseaseCategories_DiseaseCategoryId",
                table: "PatientVisits");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisits_DiseaseCategoryId",
                table: "PatientVisits");

            migrationBuilder.DropColumn(
                name: "DiseaseCategoryId",
                table: "PatientVisits");

            migrationBuilder.AddColumn<string>(
                name: "Disease",
                table: "PatientVisits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
