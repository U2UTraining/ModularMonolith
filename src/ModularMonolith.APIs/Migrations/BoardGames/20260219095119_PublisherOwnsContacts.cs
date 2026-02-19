using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.BoardGames
{
    /// <inheritdoc />
    public partial class PublisherOwnsContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Publishers_PublisherId",
                schema: "games",
                table: "Contact");

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

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                schema: "games",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "games",
                table: "Contact",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 0)
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Publishers_PublisherId",
                schema: "games",
                table: "Contact",
                column: "PublisherId",
                principalSchema: "games",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Publishers_PublisherId",
                schema: "games",
                table: "Contact");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                schema: "games",
                table: "Contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "games",
                table: "Contact",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Publishers_PublisherId",
                schema: "games",
                table: "Contact",
                column: "PublisherId",
                principalSchema: "games",
                principalTable: "Publishers",
                principalColumn: "Id");
        }
    }
}
