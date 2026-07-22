using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Common.IntegrationEvents
{
    public sealed record RestaurantRejectedIntegrationEvent(
    Guid RestaurantId,
    Guid OwnerId,
    string RestaurantName,
    string Reason,
    DateTime RejectedAtUtc);
}
