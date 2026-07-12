using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Categories;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.WorkingHours;

namespace Restaurant.Infrastructure.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Restaurants.Restaurant> Restaurants => Set<Domain.Restaurants.Restaurant>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<AddOn> AddOns => Set<AddOn>();
    public DbSet<WorkingHour> WorkingHours => Set<WorkingHour>();
    public DbSet<DeliveryZone> DeliveryZones => Set<DeliveryZone>();
    public DbSet<RestaurantImage> RestaurantImages => Set<RestaurantImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RestaurantDbContext).Assembly);
    }
}