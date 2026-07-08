using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Restaurant.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApiDocumentation()
            .AddAppCors(configuration)
            .AddAppOutputCaching()
            .AddAppHealthChecks(configuration);

        return services;
    }

    private static IServiceCollection AddApiDocumentation(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Restaurant Service API"
            });
        });

        return services;
    }

    private static IServiceCollection AddAppCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultCorsPolicy", builder =>
            {
                var allowedOrigins =
                    configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? [];

                if (allowedOrigins.Length > 0)
                {
                    builder.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddAppOutputCaching(
        this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy(
                "DefaultCache",
                policy => policy.Expire(TimeSpan.FromMinutes(1)));
        });

        return services;
    }

    private static IServiceCollection AddAppHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionMode = configuration.GetValue<string>("ConnectionMode");

        var connectionString =
            connectionMode == "Prod"
                ? configuration.GetConnectionString("ProdCS")
                : configuration.GetConnectionString("DevCS");

        services
            .AddHealthChecks()
            .AddSqlServer(connectionString!);

        return services;
    }
}