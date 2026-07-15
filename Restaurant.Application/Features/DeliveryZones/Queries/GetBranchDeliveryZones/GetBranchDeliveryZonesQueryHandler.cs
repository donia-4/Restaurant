using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetBranchDeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetBranchDeliveryZones
{
    public sealed class GetBranchDeliveryZonesQueryHandler(
    IDeliveryZoneRepository deliveryZoneRepository)
    : IRequestHandler<GetBranchDeliveryZonesQuery, Result<List<GetBranchDeliveryZonesResponse>>>
    {
        public async Task<Result<List<GetBranchDeliveryZonesResponse>>> Handle(
            GetBranchDeliveryZonesQuery query,
            CancellationToken cancellationToken)
        {
            var zones = await deliveryZoneRepository.GetByBranchIdAsync(
                query.BranchId,
                cancellationToken);

            return zones
                .Select(z => new GetBranchDeliveryZonesResponse(
                    z.Id,
                    z.ZoneName,
                    z.DeliveryFee,
                    z.MinimumOrder))
                .ToList();
        }
    }
}
