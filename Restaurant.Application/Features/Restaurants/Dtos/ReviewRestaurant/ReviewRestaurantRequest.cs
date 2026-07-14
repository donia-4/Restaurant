using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant
{
    /// <summary>
    /// Admin review request for restaurant approval workflow.
    /// Status: Approved | Rejected | Pending (Request Modification)
    /// </summary>
    public sealed record ReviewRestaurantRequest(
        RestaurantStatus Status,
        string? Reason = null
    );
}
