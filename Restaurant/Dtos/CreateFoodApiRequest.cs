namespace Restaurant.API.Dtos
{
    public sealed record CreateFoodApiRequest(
    Guid RestaurantId,
    Guid CategoryId,
    string Name,
    string Description,
    decimal Price,
    IFormFile? Image,
    int PreparationTimeMinutes,
    int? Calories);
}
