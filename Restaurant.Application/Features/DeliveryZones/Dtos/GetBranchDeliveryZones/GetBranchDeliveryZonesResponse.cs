using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.DeliveryZones.Dtos.GetBranchDeliveryZones;

public sealed record GetBranchDeliveryZonesResponse(
    Guid Id,
    string ZoneName,
    decimal DeliveryFee,
    decimal MinimumOrder
);

