using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.API.Dtos;

public sealed record CreateRestaurantApiRequest(
    Guid OwnerId,
    string Name,
    string Description,
    IFormFile? Logo,
    IFormFile? CoverImage,
    string Phone,
    string Email,
    CuisineType CuisineType,
    string Address);
