using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class NewTableDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Managers",
            columns: new[] { "IdM", "Nume", "Prenume", "IdT"},
            values: new object[,]
            {
                { 1, "Moldovan", "Mihaela", 1 },
                { 2, "Sirbu", "Dragos", 2 }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "IdM",
                keyValue: 1
            );


            migrationBuilder.DeleteData(
               table: "Managers",
               keyColumn: "IdM",
               keyValue: 2
           );

        }
    }
}
