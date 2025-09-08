using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poker.Game.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class isDisconnected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_disconnected",
                schema: "tables",
                table: "players",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_disconnected",
                schema: "tables",
                table: "players");
        }
    }
}
