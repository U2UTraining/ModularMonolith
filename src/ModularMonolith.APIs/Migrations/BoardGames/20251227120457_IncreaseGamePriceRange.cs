using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.BoardGames
{
    /// <inheritdoc />
    public partial class IncreaseGamePriceRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price_Amount",
                schema: "games",
                table: "Games",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price_Amount",
                schema: "games",
                table: "Games",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }
    }
}
