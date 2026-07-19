using MediatR;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetBranchDeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetBranchDeliveryZones;

public sealed record GetBranchDeliveryZonesQuery(
    Guid BranchId,
    int PageNumber = 1,
    int PageSize = 10)
    : IRequest<Result<PaginatedList<GetBranchDeliveryZonesResponse>>>,
      ICachedQuery
{
    public string CacheKey =>
        $"branch-delivery-zones:{BranchId}:{PageNumber}:{PageSize}";

    public string[] Tags =>
    [
        "delivery-zones",
        $"branch:{BranchId}"
    ];

    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}