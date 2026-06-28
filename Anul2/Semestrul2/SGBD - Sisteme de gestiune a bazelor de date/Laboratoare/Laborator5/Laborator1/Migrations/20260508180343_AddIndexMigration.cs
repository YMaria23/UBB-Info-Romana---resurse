using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "index_TeatruNational_Oras_Denumire",
                table: "TeatruNational",
                columns: new[] { "Oras", "Denumire" });

            migrationBuilder.CreateIndex(
                name: "index_Managers_Name",
                table: "Managers",
                column: "Nume");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "index_TeatruNational_Oras_Denumire",
                table: "TeatruNational");

            migrationBuilder.DropIndex(
                name: "index_Managers_Name",
                table: "Managers");
        }
    }
}
