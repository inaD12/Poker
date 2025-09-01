using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poker.Game.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGamesWonandGamesPlayedtoHandsWonandHandsPlayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "games_won",
                schema: "tables",
                table: "players",
                newName: "hands_won");

            migrationBuilder.RenameColumn(
                name: "games_played",
                schema: "tables",
                table: "players",
                newName: "hands_played");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hands_won",
                schema: "tables",
                table: "players",
                newName: "games_won");

            migrationBuilder.RenameColumn(
                name: "hands_played",
                schema: "tables",
                table: "players",
                newName: "games_played");
        }
    }
}
