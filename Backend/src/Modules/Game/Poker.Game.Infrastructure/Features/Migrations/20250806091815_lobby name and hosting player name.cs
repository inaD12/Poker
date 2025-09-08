using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poker.Game.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class lobbynameandhostingplayername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hosting_player_name",
                schema: "tables",
                table: "lobbies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "tables",
                table: "lobbies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hosting_player_name",
                schema: "tables",
                table: "lobbies");

            migrationBuilder.DropColumn(
                name: "name",
                schema: "tables",
                table: "lobbies");
        }
    }
}
