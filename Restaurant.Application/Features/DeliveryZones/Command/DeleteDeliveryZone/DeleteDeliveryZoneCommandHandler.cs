using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.DeleteDeliveryZone
{
    public sealed class DeleteDeliveryZoneCommandHandler(
    IDeliveryZoneRepository deliveryZoneRepository,
    ICacheService cacheService)
    : IRequestHandler<DeleteDeliveryZoneCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(
            DeleteDeliveryZoneCommand command,
            CancellationToken cancellationToken)
        {
            var zone = await deliveryZoneRepository.GetByIdAsync(
                command.ZoneId,
                cancellationToken);

            if (zone is null)
                return DeliveryZoneErrors.NotFound;

            var branchId = zone.BranchId;

            await deliveryZoneRepository.DeleteAsync(zone, cancellationToken);
            await deliveryZoneRepository.SaveChangesAsync(cancellationToken);

            await cacheService.RemoveAsync($"deliveryzone:{command.ZoneId}", cancellationToken);
            await cacheService.RemoveAsync($"branch-delivery-zones:{branchId}", cancellationToken);

            return Result.Deleted;
        }
    }
}
