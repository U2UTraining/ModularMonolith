using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularMonolith.APIs.Migrations.BoardGames
{
    /// <inheritdoc />
    public partial class RenameGamesEntityToBoardGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Games_Id",
                schema: "games",
                table: "GameImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_PublisherId",
                schema: "games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                schema: "games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Games",
                schema: "games",
                newName: "BoardGames",
                newSchema: "games");

            migrationBuilder.RenameIndex(
                name: "IX_Games_PublisherId",
                schema: "games",
                table: "BoardGames",
                newName: "IX_BoardGames_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_Name",
                schema: "games",
                table: "BoardGames",
                newName: "IX_BoardGames_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardGames",
                schema: "games",
                table: "BoardGames",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                schema: "games",
                table: "BoardGames",
                column: "PublisherId",
                principalSchema: "games",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_BoardGames_Id",
                schema: "games",
                table: "GameImage",
                column: "Id",
                principalSchema: "games",
                principalTable: "BoardGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                schema: "games",
                table: "BoardGames");

            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_BoardGames_Id",
                schema: "games",
                table: "GameImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardGames",
                schema: "games",
                table: "BoardGames");

            migrationBuilder.RenameTable(
                name: "BoardGames",
                schema: "games",
                newName: "Games",
                newSchema: "games");

            migrationBuilder.RenameIndex(
                name: "IX_BoardGames_PublisherId",
                schema: "games",
                table: "Games",
                newName: "IX_Games_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardGames_Name",
                schema: "games",
                table: "Games",
                newName: "IX_Games_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                schema: "games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Games_Id",
                schema: "games",
                table: "GameImage",
                column: "Id",
                principalSchema: "games",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_PublisherId",
                schema: "games",
                table: "Games",
                column: "PublisherId",
                principalSchema: "games",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
