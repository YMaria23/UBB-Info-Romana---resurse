using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresa",
                table: "TeatruNational",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresa",
                table: "TeatruNational");
        }
    }
}
