using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetDeliveryZone;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetDeliveryZone
{
    public sealed class GetDeliveryZoneQueryHandler(
    IDeliveryZoneRepository deliveryZoneRepository)
    : IRequestHandler<GetDeliveryZoneQuery, Result<GetDeliveryZoneResponse>>
    {
        public async Task<Result<GetDeliveryZoneResponse>> Handle(
            GetDeliveryZoneQuery query,
            CancellationToken cancellationToken)
        {
            var zone = await deliveryZoneRepository.GetByIdAsync(
                query.ZoneId,
                cancellationToken);

            if (zone is null)
                return DeliveryZoneErrors.NotFound;

            return new GetDeliveryZoneResponse(
                zone.Id,
                zone.BranchId,
                zone.ZoneName,
                zone.DeliveryFee,
                zone.MinimumOrder,
                zone.PolygonGeoJson);
        }
    }
}
