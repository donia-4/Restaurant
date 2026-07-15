using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetBranchDeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetBranchDeliveryZones
{
    public sealed record GetBranchDeliveryZonesQuery(Guid BranchId)
    : IRequest<Result<List<GetBranchDeliveryZonesResponse>>>, ICachedQuery
    {
        public string CacheKey => $"branch-delivery-zones:{BranchId}";
        public string[] Tags => ["delivery-zones", $"branch:{BranchId}"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
