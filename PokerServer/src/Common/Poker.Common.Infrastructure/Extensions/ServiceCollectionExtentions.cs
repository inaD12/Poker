using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Options;
using Poker.Common.Infrastructure.Clock;

namespace Poker.Common.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddDatabaseContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<NpgsqlDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
    {
        services
            .AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var databaseOptions = configuration
            .GetSection(nameof(DatabaseOptions))
            .Get<DatabaseOptions>()!;

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(
                databaseOptions.ConnectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                    optionsAction?.Invoke(npgsqlOptions);
                }).UseSnakeCaseNamingConvention()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<TContext>();

        return services;
    }
}