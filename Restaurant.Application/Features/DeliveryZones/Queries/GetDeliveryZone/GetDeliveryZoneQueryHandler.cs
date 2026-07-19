using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetDeliveryZone;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetDeliveryZone
{
    public sealed class GetDeliveryZoneQueryHandler(
    IDeliveryZoneRepository deliveryZoneRepository, ILogger<GetDeliveryZoneQueryHandler> logger)
    : IRequestHandler<GetDeliveryZoneQuery, Result<GetDeliveryZoneResponse>>
    {
        public async Task<Result<GetDeliveryZoneResponse>> Handle(
            GetDeliveryZoneQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Processing GetDeliveryZoneQuery for Zone ID: {ZoneId}",
                query.ZoneId);

            var zone = await deliveryZoneRepository.GetByIdAsync(
                query.ZoneId,
                cancellationToken);

            if (zone is null)
                return DeliveryZoneErrors.NotFound;

            logger.LogInformation(
                "Successfully retrieved delivery zone with ID: {ZoneId}",
                query.ZoneId);

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
