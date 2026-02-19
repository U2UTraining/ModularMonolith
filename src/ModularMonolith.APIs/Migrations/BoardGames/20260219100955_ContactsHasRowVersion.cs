using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.BoardGames
{
    /// <inheritdoc />
    public partial class ContactsHasRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "games",
                table: "Contact",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 2147483647);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "games",
                table: "Contact",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcCreated",
                schema: "games",
                table: "Contact",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()")
                .Annotation("Relational:ColumnOrder", 2147483645);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcDeleted",
                schema: "games",
                table: "Contact",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()")
                .Annotation("Relational:ColumnOrder", 2147483646);

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcModified",
                schema: "games",
                table: "Contact",
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
                schema: "games",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "games",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UtcCreated",
                schema: "games",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UtcDeleted",
                schema: "games",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UtcModified",
                schema: "games",
                table: "Contact");
        }
    }
}
