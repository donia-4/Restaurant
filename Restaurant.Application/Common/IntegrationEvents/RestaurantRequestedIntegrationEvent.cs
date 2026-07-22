using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Common.IntegrationEvents
{
    public sealed record RestaurantRequestedIntegrationEvent(
    Guid RestaurantId,
    Guid OwnerId,
    string RestaurantName,
    DateTime OccurredOnUtc);
}
