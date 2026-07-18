using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;

public sealed record UpdateRestaurantResponse(
    Guid Id,
    string Name,
    string Description,
    string Phone,
    string Email,
    CuisineType CuisineType,
    string Address,
    string? Logo,
    string? CoverImage);