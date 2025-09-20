using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolithMigrations.BoardGames
{
    /// <inheritdoc />
    public partial class BoardGamesInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "games");

            migrationBuilder.CreateTable(
                name: "Publishers",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UtcModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcDeleted = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                    table.UniqueConstraint("AK_Publishers_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    UtcModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcDeleted = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublisherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalSchema: "games",
                        principalTable: "Publishers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Price_Amount = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Price_Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UtcModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcDeleted = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalSchema: "games",
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameImage",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ImageLocation = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    UtcModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UtcDeleted = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameImage_Games_Id",
                        column: x => x.Id,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PublisherId",
                schema: "games",
                table: "Contact",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                schema: "games",
                table: "Games",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PublisherId",
                schema: "games",
                table: "Games",
                column: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameImage",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Publishers",
                schema: "games");
        }
    }
}
