using System.Reflection;
using Poker.Common.Presentation.Endpoints;
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

var assembly = Assembly.GetExecutingAssembly();

builder.Services
    .AddGameModule(config)
    .AddUsersModule(config)
    .AddApiLayer(config);

var app = builder.Build();

await app.SetUpDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poker API V1"); });
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/hubs/game");
app.MapHub<LobbyHub>("/hubs/lobby");

app.MapEndpoints();

app.Run();
