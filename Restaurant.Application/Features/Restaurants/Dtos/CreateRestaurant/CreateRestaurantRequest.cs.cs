using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Dtos;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant
{
    public sealed record CreateRestaurantRequest(
        Guid OwnerId,
        string Name,
        string Description,
        FileUpload? Logo,
        FileUpload? CoverImage,
        string Phone,
        string Email,
        CuisineType CuisineType,
        string Address);
}
