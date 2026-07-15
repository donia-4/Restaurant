using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.DeliveryZones.Dtos.CreateDeliveryZone;
using Restaurant.Domain.Branches;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.CreateDeliveryZone
{
    public sealed class CreateDeliveryZoneCommandHandler(
    IDeliveryZoneRepository deliveryZoneRepository,
    IBranchRepository branchRepository,
    ICacheService cacheService)
    : IRequestHandler<CreateDeliveryZoneCommand, Result<CreateDeliveryZoneResponse>>
    {
        public async Task<Result<CreateDeliveryZoneResponse>> Handle(
            CreateDeliveryZoneCommand command,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            var branch = await branchRepository.GetByIdAsync(
                request.BranchId,
                cancellationToken);

            if (branch is null)
                return BranchErrors.NotFound;

            var exists = await deliveryZoneRepository.ExistsAsync(
                request.BranchId,
                request.ZoneName,
                cancellationToken);

            if (exists)
                return DeliveryZoneErrors.DuplicateName;

            var zoneResult = DeliveryZone.Create(
                Guid.NewGuid(),
                request.BranchId,
                request.ZoneName,
                request.DeliveryFee,
                request.MinimumOrder,
                request.PolygonGeoJson);

            if (zoneResult.IsError)
                return zoneResult.Errors;

            await deliveryZoneRepository.AddAsync(zoneResult.Value, cancellationToken);
            await deliveryZoneRepository.SaveChangesAsync(cancellationToken);

            await cacheService.RemoveAsync(
                $"branch-delivery-zones:{request.BranchId}",
                cancellationToken);

            return new CreateDeliveryZoneResponse(zoneResult.Value.Id);
        }
    }
}
