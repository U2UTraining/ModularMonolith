using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace U2U.ModularMonolith.Migrations.Currencies
{
    /// <inheritdoc />
    public partial class CurrenciesInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "currencies");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ValueInEuro = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UtcModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcDeleted = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.CheckConstraint("CK_Value_Positive", "ValueInEuro > 0");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "currencies");
        }
    }
}
