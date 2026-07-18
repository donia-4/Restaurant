using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.API.Dtos
{
    public sealed record UpdateRestaurantApiRequest(
    string? Name,
    string? Description,
    string? Phone,
    string? Email,
    CuisineType? CuisineType,
    string? Address,
    IFormFile? Logo,
    IFormFile? CoverImage);
}
