using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.RequestRestaurantModification
{
    public sealed record RequestRestaurantModificationResponse(
        Guid Id,
        string Name,
        string Status,
        bool IsApproved);
}
