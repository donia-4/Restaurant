using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetDeliveryZone;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetDeliveryZone
{
    public sealed record GetDeliveryZoneQuery(Guid ZoneId)
    : IRequest<Result<GetDeliveryZoneResponse>>, ICachedQuery
    {
        public string CacheKey => $"deliveryzone:{ZoneId}";
        public string[] Tags => ["delivery-zones"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
