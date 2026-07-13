using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Infrastructure.Services;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Data.Interceptors;
using Restaurant.Infrastructure.Repositories;

namespace Restaurant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddCaching()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<RestaurantDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);

            options.AddInterceptors(
                sp.GetServices<ISaveChangesInterceptor>());
        });

        return services;
    }

    private static IServiceCollection AddCaching(
        this IServiceCollection services)
    {
        services.AddHybridCache();

        services.AddScoped<ICacheService, HybridCacheService>();

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IFoodRepository, FoodRepository>();

        services.AddScoped<IBranchRepository, BranchRepository>();

        return services;
    }
}