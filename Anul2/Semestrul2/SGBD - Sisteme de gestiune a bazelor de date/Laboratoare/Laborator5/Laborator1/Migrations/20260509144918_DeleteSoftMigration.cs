using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laborator1.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSoftMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Spectacole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Spectacole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Spectacole",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Spectacole");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Spectacole");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Spectacole");
        }
    }
}
