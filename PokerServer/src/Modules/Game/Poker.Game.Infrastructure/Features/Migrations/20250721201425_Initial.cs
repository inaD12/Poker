using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poker.Game.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tables");

            migrationBuilder.CreateTable(
                name: "lobbies",
                schema: "tables",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobbies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "table_snapshots",
                schema: "tables",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    TableJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_table_snapshots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                schema: "tables",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    lobby_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                    table.ForeignKey(
                        name: "fk_players_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalSchema: "tables",
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_players_lobby_id",
                schema: "tables",
                table: "players",
                column: "lobby_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "players",
                schema: "tables");

            migrationBuilder.DropTable(
                name: "table_snapshots",
                schema: "tables");

            migrationBuilder.DropTable(
                name: "lobbies",
                schema: "tables");
        }
    }
}
