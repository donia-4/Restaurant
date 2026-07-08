using Restaurant.Domain.Categories;
using Restaurant.Domain.Common;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;
using Restaurant.Domian.Foods;

namespace Restaurant.Domain.Foods;

public sealed class Food : AuditableEntity
{
    public Guid RestaurantId { get; private set; }

    public Guid CategoryId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public string? Image { get; private set; }

    public bool Available { get; private set; }

    // Navigation Properties
    public Restaurants.Restaurant Restaurant { get; private set; } = null!;

    public Category Category { get; private set; } = null!;

    private Food()
    {
    }

    private Food(
        Guid id,
        Guid restaurantId,
        Guid categoryId,
        string name,
        string description,
        decimal price,
        string? image)
        : base(id)
    {
        RestaurantId = restaurantId;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
        Image = image;
        Available = true;
    }

    public static Result<Food> Create(
        Guid id,
        Guid restaurantId,
        Guid categoryId,
        string name,
        string description,
        decimal price,
        string? image = null)
    {
        if (restaurantId == Guid.Empty)
            return FoodErrors.InvalidRestaurant;

        if (categoryId == Guid.Empty)
            return FoodErrors.InvalidCategory;

        if (string.IsNullOrWhiteSpace(name))
            return FoodErrors.InvalidName;

        if (price <= 0)
            return FoodErrors.InvalidPrice;

        return new Food(
            id,
            restaurantId,
            categoryId,
            name.Trim(),
            description.Trim(),
            price,
            image);
    }

    public Result<Updated> Update(
        string name,
        string description,
        decimal price,
        Guid categoryId,
        string? image)
    {
        if (string.IsNullOrWhiteSpace(name))
            return FoodErrors.InvalidName;

        if (categoryId == Guid.Empty)
            return FoodErrors.InvalidCategory;

        if (price <= 0)
            return FoodErrors.InvalidPrice;

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        CategoryId = categoryId;
        Image = image;

        return Result.Updated;
    }

    public Result<Updated> ChangeAvailability(bool available)
    {
        Available = available;

        return Result.Updated;
    }
}