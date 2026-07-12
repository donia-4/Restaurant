using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant
{
    public sealed record CreateRestaurantRequest(
        Guid OwnerId,
        string Name,
        string Description,
        string Phone,
        string Email,
        CuisineType CuisineType,
        string Address);
}
