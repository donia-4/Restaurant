using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.DeliveryZones.Dtos.UpdateDeliveryZone
{
    public sealed record UpdateDeliveryZoneRequest(
    string? ZoneName = null,
    decimal? DeliveryFee = null,
    decimal? MinimumOrder = null,
    string? PolygonGeoJson = null
);

}
