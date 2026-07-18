using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.GetAllRestaurants
{
    public sealed record GetAllRestaurantsResponse(
        Guid Id,
        string Name,
        string Description,
        string? Logo,
        string? CoverImage,
        string Phone,
        string Email,
        CuisineType CuisineType,
        string Address,
        RestaurantStatus Status,
        bool IsApproved,
        decimal AverageRating,
        int TotalReviews);
}
