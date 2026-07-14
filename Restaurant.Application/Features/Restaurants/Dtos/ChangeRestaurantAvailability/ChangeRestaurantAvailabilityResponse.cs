using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Restaurants.Dtos.ChangeRestaurantAvailability
{
    public sealed record ChangeRestaurantAvailabilityResponse(
        Guid RestaurantId,
        string Status,
        string Message
    );
}
