using Restaurant.Domain.Results;

namespace Restaurant.Domain.Categories;

public static class CategoryErrors
{
    public static readonly Error InvalidRestaurant = Error.Validation("Category.Restaurant.Required", "Restaurant is required.");

    public static readonly Error InvalidName = Error.Validation("Category.Name.Required", "Category name is required.");

    public static readonly Error NotFound =  Error.NotFound("Category.NotFound", "Category was not found.");
}