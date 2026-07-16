namespace Restaurant.API.Dtos
{
    public sealed record UpdateFoodApiRequest(
        string? Name,
        string? Description,
        decimal? Price,
        Guid? CategoryId,
        IFormFile? Image,
        int? PreparationTimeMinutes,
        int? Calories);
}
