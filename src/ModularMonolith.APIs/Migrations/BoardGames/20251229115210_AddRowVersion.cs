using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.BoardGames
{
    /// <inheritdoc />
    public partial class AddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "games",
                table: "Publishers",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "games",
                table: "Games",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "games",
                table: "GameImage",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "games",
                table: "Contact",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "games",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "games",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "games",
                table: "GameImage");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "games",
                table: "Contact");
        }
    }
}
