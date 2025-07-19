using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Options;
using Poker.Common.Utilities;
using PokerServer.Notifiers;
using PokerServer.Options;

namespace PokerServer.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApiLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection
			.AddTransient<ILobbyNotifier, LobbyNotifier>()
			.AddTransient<ITableNotifier, TableNotifier>()
			.AddSignalR();

		serviceCollection
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.AddControllers();

		return serviceCollection;
	}
	
	public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOptions<AuthOptions>()
			.BindConfiguration(nameof(AuthOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var tokenOptions = configuration
			.GetSection(nameof(AuthOptions))
			.Get<AuthOptions>()!;

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(opts =>
			{
				byte[] signingKeyBytes = Encoding.UTF8
					.GetBytes(tokenOptions.SecretKey);

				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = tokenOptions.Issuer,
					ValidateAudience = true,
					ValidAudience = tokenOptions.Audience,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
				};
			});

		services
			.AddAuthorization();

		return services;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			var securityScheme = new OpenApiSecurityScheme
			{
				Name = "JWT Authentication",
				Description = "Enter JWT Bearer token **_only_**",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT",
				Reference = new OpenApiReference
				{
					Id = JwtBearerDefaults.AuthenticationScheme,
					Type = ReferenceType.SecurityScheme
				}
			};
			options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{securityScheme, [] }
			});
		});

		return services;
	}

	public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
	{
		services
		   .AddOptions<CorsOptions>()
		   .BindConfiguration(nameof(CorsOptions))
		   .ValidateDataAnnotations()
		   .ValidateOnStart();

		var corsOptions = configuration
			.GetSection(nameof(CorsOptions))
			.Get<CorsOptions>()!;

		services.AddCors(options =>
		{
			options.AddDefaultPolicy(builder =>
			{
				builder.AllowAnyOrigin()
					   .AllowAnyMethod()
					   .AllowAnyHeader();
			});

			options.AddPolicy(AppPolicies.CorsPolicy, builder =>
			   builder.WithOrigins(corsOptions.AllowedOrigins.Split(", "))
					  .AllowAnyHeader()
					  .AllowAnyMethod());
		});

		return services;
	}
}
