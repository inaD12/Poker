using Poker.Common.Presentation.Endpoints;
using Poker.Common.Utilities;
using Poker.Game.Presentation.Extensions;
using Poker.Users.Presentation.Extensions;
using PokerServer.Extensions;
using PokerServer.Hubs;
using Serilog;

//TODO: change host when original one leaves,
//delete game/lobby if left empty/not enough players,
//StopGame only after showdown,
//LeaveGame,
//Game reconnection(find which game player is in),
//GetAllLobbies,
//distribute funds to users module after game,
//Optional: timeouts, chat, game history, admin controls, refunds if game is canceled, rejoin period

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();
builder.Services.AddControllers();

builder.Services
    .AddGameModule(config)
    .AddUsersModule(config)
    .AddApiLayer(config);

var app = builder.Build();

await app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poker API V1"); });
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapHub<GameHub>("/hubs/game");
app.MapHub<LobbyHub>("/hubs/lobby");

app.MapEndpoints();

app.Run();