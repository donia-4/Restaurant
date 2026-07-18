using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Data.Interceptors;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Infrastructure.Services;
using Restaurant.Infrastructure.Settings;

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
            .AddCloudinary(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddCloudinary(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(
            configuration.GetSection("Cloudinary"));

        services.AddSingleton(sp =>
        {
            var settings = sp
                .GetRequiredService<IOptions<CloudinarySettings>>()
                .Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret);

            return new Cloudinary(account);
        });

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

        services.AddScoped<IFileService, CloudinaryFileService>();

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IFoodRepository, FoodRepository>();

        services.AddScoped<IBranchRepository, BranchRepository>();

        services.AddScoped<IAddOnRepository, AddOnRepository>();

        services.AddScoped<IDeliveryZoneRepository, DeliveryZoneRepository>();

        return services;
    }
}