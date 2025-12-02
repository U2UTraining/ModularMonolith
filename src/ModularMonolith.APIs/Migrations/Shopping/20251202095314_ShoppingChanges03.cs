using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.Shopping
{
    /// <inheritdoc />
    public partial class ShoppingChanges03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "shopping",
                table: "BasketItems",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 2147483647);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcCreated",
                schema: "shopping",
                table: "BasketItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()")
                .Annotation("Relational:ColumnOrder", 2147483645);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcDeleted",
                schema: "shopping",
                table: "BasketItems",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()")
                .Annotation("Relational:ColumnOrder", 2147483646);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcModified",
                schema: "shopping",
                table: "BasketItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()")
                .Annotation("Relational:ColumnOrder", 2147483644);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "shopping",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "UtcCreated",
                schema: "shopping",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "UtcDeleted",
                schema: "shopping",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "UtcModified",
                schema: "shopping",
                table: "BasketItems");
        }
    }
}
