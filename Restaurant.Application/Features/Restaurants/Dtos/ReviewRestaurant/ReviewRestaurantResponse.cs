using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant
{
    public sealed record ReviewRestaurantResponse(
        Guid RestaurantId,
        string Status,
        string Message
    );
}
