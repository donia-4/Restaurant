using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.ChangeRestaurantAvailability
{
    /// <summary>
    /// Restaurant owner availability change request.
    /// Status: Open | Closed | TemporarilyClosed | UnderMaintenance
    /// </summary>
    public sealed record ChangeRestaurantAvailabilityRequest(
        RestaurantStatus Status
    );
}
