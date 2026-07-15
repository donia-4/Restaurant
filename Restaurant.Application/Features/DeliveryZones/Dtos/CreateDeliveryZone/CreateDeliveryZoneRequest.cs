using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.DeliveryZones.Commands.CreateDeliveryZone;

public sealed record CreateDeliveryZoneRequest(
    Guid BranchId,
    string ZoneName,
    decimal DeliveryFee,
    decimal MinimumOrder,
    string? PolygonGeoJson = null
);
