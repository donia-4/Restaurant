using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Foods;

namespace Restaurant.Infrastructure.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Restaurants.Restaurant> Restaurants => Set<Domain.Restaurants.Restaurant>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Category> Categories => Set<Category>();
    //public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    //public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantDbContext).Assembly);
    }
}