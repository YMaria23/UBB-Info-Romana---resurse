using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeatruNational",
                columns: table => new
                {
                    IdT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Oras = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnInfiintare = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeatruNational", x => x.IdT);
                });

            migrationBuilder.CreateTable(
                name: "Spectacole",
                columns: table => new
                {
                    IdS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spectacole", x => x.IdS);
                    table.ForeignKey(
                        name: "FK_Spectacole_TeatruNational_IdT",
                        column: x => x.IdT,
                        principalTable: "TeatruNational",
                        principalColumn: "IdT",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spectacole_IdT",
                table: "Spectacole",
                column: "IdT");

            migrationBuilder.CreateIndex(
                name: "IX_TeatruNational_Denumire",
                table: "TeatruNational",
                column: "Denumire",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spectacole");

            migrationBuilder.DropTable(
                name: "TeatruNational");
        }
    }
}
