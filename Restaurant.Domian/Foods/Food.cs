using Restaurant.Domain.AddOns;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Common;
using Restaurant.Domain.Results;

namespace Restaurant.Domain.Foods;

public sealed class Food : AuditableEntity
{
    private readonly List<AddOn> _addOns = [];

    public Guid RestaurantId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string? Image { get; private set; }
    public int PreparationTimeMinutes { get; private set; } // FR-06
    public int? Calories { get; private set; } // FR-06 (optional)
    public bool IsAvailable { get; private set; }
    public bool IsVisible { get; private set; } // FR-06: إخفاء/إظهار

    public Restaurants.Restaurant Restaurant { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public IReadOnlyCollection<AddOn> AddOns => _addOns;

    private Food() { }

    private Food(Guid id, Guid restaurantId, Guid categoryId, string name,
        string description, decimal price, string? image, int preparationTimeMinutes, int? calories) : base(id)
    {
        RestaurantId = restaurantId; CategoryId = categoryId; Name = name;
        Description = description; Price = price; Image = image;
        PreparationTimeMinutes = preparationTimeMinutes; Calories = calories;
        IsAvailable = true; IsVisible = true;
    }

    public static Result<Food> Create(Guid id, Guid restaurantId, Guid categoryId, string name,
        string description, decimal price, string? image = null, int preparationTimeMinutes = 0, int? calories = null)
    {
        if (restaurantId == Guid.Empty) return FoodErrors.InvalidRestaurant;
        if (categoryId == Guid.Empty) return FoodErrors.InvalidCategory; // BR-07
        if (string.IsNullOrWhiteSpace(name)) return FoodErrors.InvalidName;
        if (price < 0) return FoodErrors.InvalidPrice; // BR-06

        return new Food(id, restaurantId, categoryId, name.Trim(), description.Trim(),
            price, image, preparationTimeMinutes, calories);
    }

    public Result<Updated> Update(string name, string description, decimal price,
        Guid categoryId, string? image, int preparationTimeMinutes, int? calories)
    {
        if (string.IsNullOrWhiteSpace(name)) return FoodErrors.InvalidName;
        if (categoryId == Guid.Empty) return FoodErrors.InvalidCategory;
        if (price < 0) return FoodErrors.InvalidPrice;

        Name = name.Trim(); Description = description.Trim(); Price = price;
        CategoryId = categoryId; Image = image;
        PreparationTimeMinutes = preparationTimeMinutes; Calories = calories;
        return Result.Updated;
    }

    public Result<Updated> ChangeAvailability(bool available) { IsAvailable = available; return Result.Updated; }
    public Result<Updated> Hide() { IsVisible = false; return Result.Updated; }
    public Result<Updated> Show() { IsVisible = true; return Result.Updated; }

    public Result<Updated> AddAddOn(AddOn addOn) { _addOns.Add(addOn); return Result.Updated; }
    public Result<Updated> RemoveAddOn(AddOn addOn) { _addOns.Remove(addOn); return Result.Updated; }
}