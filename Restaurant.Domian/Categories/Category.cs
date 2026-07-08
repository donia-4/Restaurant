using Restaurant.Domain.Common;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Domain.Categories;

public sealed class Category : AuditableEntity
{
    private readonly List<Food> _foods = [];

    public Guid RestaurantId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public Restaurants.Restaurant Restaurant { get; private set; } = null!;

    public IReadOnlyCollection<Food> Foods => _foods;

    private Category()
    {
    }

    private Category(
        Guid id,
        Guid restaurantId,
        string name)
        : base(id)
    {
        RestaurantId = restaurantId;
        Name = name;
    }

    public static Result<Category> Create(
        Guid id,
        Guid restaurantId,
        string name)
    {
        if (restaurantId == Guid.Empty)
            return CategoryErrors.InvalidRestaurant;

        if (string.IsNullOrWhiteSpace(name))
            return CategoryErrors.InvalidName;

        return new Category(
            id,
            restaurantId,
            name.Trim());
    }

    public Result<Updated> Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CategoryErrors.InvalidName;

        Name = name.Trim();

        return Result.Updated;
    }
}