using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poker.Game.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class createdAtfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "tables",
                table: "table_snapshots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "tables",
                table: "players",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "games_played",
                schema: "tables",
                table: "players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "games_won",
                schema: "tables",
                table: "players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_earnings",
                schema: "tables",
                table: "players",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "tables",
                table: "lobbies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "hosting_player_id",
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
                name: "created_at",
                schema: "tables",
                table: "table_snapshots");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "tables",
                table: "players");

            migrationBuilder.DropColumn(
                name: "games_played",
                schema: "tables",
                table: "players");

            migrationBuilder.DropColumn(
                name: "games_won",
                schema: "tables",
                table: "players");

            migrationBuilder.DropColumn(
                name: "total_earnings",
                schema: "tables",
                table: "players");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "tables",
                table: "lobbies");

            migrationBuilder.DropColumn(
                name: "hosting_player_id",
                schema: "tables",
                table: "lobbies");
        }
    }
}
