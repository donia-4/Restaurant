using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.DeliveryZones.Dtos.UpdateDeliveryZone;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.UpdateDeliveryZone
{
    public sealed class UpdateDeliveryZoneCommandHandler(
    IDeliveryZoneRepository deliveryZoneRepository,
    ICacheService cacheService)
    : IRequestHandler<UpdateDeliveryZoneCommand, Result<UpdateDeliveryZoneResponse>>
    {
        public async Task<Result<UpdateDeliveryZoneResponse>> Handle(
            UpdateDeliveryZoneCommand command,
            CancellationToken cancellationToken)
        {
            var zone = await deliveryZoneRepository.GetByIdAsync(
                command.ZoneId,
                cancellationToken);

            if (zone is null)
                return DeliveryZoneErrors.NotFound;

            if (command.Request.ZoneName is not null &&
                command.Request.ZoneName != zone.ZoneName)
            {
                var exists = await deliveryZoneRepository.ExistsAsync(
                    zone.BranchId,
                    command.Request.ZoneName,
                    cancellationToken);

                if (exists)
                    return DeliveryZoneErrors.DuplicateName;
            }

            var updateResult = zone.Update(
                command.Request.ZoneName ?? zone.ZoneName,
                command.Request.DeliveryFee ?? zone.DeliveryFee,
                command.Request.MinimumOrder ?? zone.MinimumOrder,
                command.Request.PolygonGeoJson ?? zone.PolygonGeoJson);

            if (updateResult.IsError)
                return updateResult.Errors;

            await deliveryZoneRepository.UpdateAsync(zone, cancellationToken);
            await deliveryZoneRepository.SaveChangesAsync(cancellationToken);

            await cacheService.RemoveAsync($"deliveryzone:{zone.Id}", cancellationToken);
            await cacheService.RemoveAsync($"branch-delivery-zones:{zone.BranchId}", cancellationToken);

            return new UpdateDeliveryZoneResponse(
                zone.Id,
                zone.ZoneName,
                zone.DeliveryFee,
                zone.MinimumOrder,
                zone.PolygonGeoJson);
        }
    }
}
