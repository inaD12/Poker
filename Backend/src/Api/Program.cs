using Poker.Common.Presentation.Endpoints;
using Poker.Common.Utilities;
using Poker.Game.Presentation.Extensions;
using Poker.Users.Presentation.Extensions;
using PokerServer.Extensions;
using PokerServer.Hubs;
using Serilog;

//TODO:
//kick, reconnect
//LobbyName validation
//Optional: timeouts, chat, game history, admin controls, refunds if game is canceled, rejoin period, extract lobby to its module?

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();
builder.Services.AddControllers();

builder.Services
    .AddGameModule(config)
    .AddUsersModule(config)
    .AddApiLayer(config);

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poker API V1");
        c.ConfigObject.AdditionalItems["withCredentials"] = true;
    });
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseCookiePolicy(); 
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapHub<GameHub>("/hubs/game");
app.MapHub<LobbyHub>("/hubs/lobby");

app.MapEndpoints();

app.Run();