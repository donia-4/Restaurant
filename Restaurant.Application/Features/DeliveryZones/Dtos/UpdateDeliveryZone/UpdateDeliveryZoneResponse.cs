using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.DeliveryZones.Dtos.UpdateDeliveryZone
{
    public sealed record UpdateDeliveryZoneResponse(
    Guid Id,
    string ZoneName,
    decimal DeliveryFee,
    decimal MinimumOrder,
    string? PolygonGeoJson
);
}
