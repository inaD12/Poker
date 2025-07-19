using Poker.Common.Utilities;
using Poker.Game.Presentation.Extensions;
using Poker.Users.Presentation.Extensions;
using PokerServer.Extensions;
using PokerServer.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();
builder.Services.AddControllers();

builder.Services
    .AddGameModule(config)
    .AddUsersModule(config)
    .AddApiLayer(config);

var app = builder.Build();

await app.SetUpDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/hubs/game");
app.MapHub<LobbyHub>("/hubs/lobby");

app.Run();
