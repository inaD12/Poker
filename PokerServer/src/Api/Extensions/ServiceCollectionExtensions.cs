using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Options;
using Poker.Common.Presentation.Abstractions;
using Poker.Common.Presentation.Helpers;
using Poker.Common.Presentation.Options;
using Poker.Common.Utilities;
using PokerServer.ExceptionHandlers;
using PokerServer.Notifiers;

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
			.AddExceptionHandling()
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.AddControllers();

		return serviceCollection;
	}
	
	private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		services
			.AddExceptionHandler<ValidationExceptionHandler>()
			.AddExceptionHandler<GlobalExceptionHandler>()
			.AddProblemDetails();
        
		return services;
	}
	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
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
			.AddAuthorization()
			.AddScoped<IClaimsExtractor, ClaimsExtractor>();

		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "Poker API",
				Version = "v1",
				Description = "API documentation for Poker server"
			});
			
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

	private static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
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
