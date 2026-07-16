using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;

public sealed record CreateRestaurantResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string Description,
    string Phone,
    string Email,
    CuisineType CuisineType,
    string Address,
    string? Logo,
    string? CoverImage,
    string Status,
    bool IsApproved);