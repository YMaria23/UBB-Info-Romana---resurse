using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class NewTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    IdM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prenume = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.IdM);
                    table.ForeignKey(
                        name: "FK_Managers_TeatruNational_IdT",
                        column: x => x.IdT,
                        principalTable: "TeatruNational",
                        principalColumn: "IdT",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdT",
                table: "Managers",
                column: "IdT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
