using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.DeliveryZones.Dtos.GetBranchDeliveryZones;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Queries.GetBranchDeliveryZones;

public sealed class GetBranchDeliveryZonesQueryHandler(
    IDeliveryZoneRepository deliveryZoneRepository,
    ILogger<GetBranchDeliveryZonesQueryHandler> logger)
    : IRequestHandler<GetBranchDeliveryZonesQuery, Result<PaginatedList<GetBranchDeliveryZonesResponse>>>
{
    public async Task<Result<PaginatedList<GetBranchDeliveryZonesResponse>>> Handle(
        GetBranchDeliveryZonesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing GetBranchDeliveryZonesQuery for Branch ID: {BranchId}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.BranchId,
            request.PageNumber,
            request.PageSize);

        var zones = await deliveryZoneRepository.GetByBranchIdAsync(
            request.BranchId,
            cancellationToken);

        var mappedZones = zones
            .Select(zone => new GetBranchDeliveryZonesResponse(
                zone.Id,
                zone.ZoneName,
                zone.DeliveryFee,
                zone.MinimumOrder))
            .ToList();

        var paginatedResponse = new PaginatedList<GetBranchDeliveryZonesResponse>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = mappedZones.Count,
            TotalPages = (int)Math.Ceiling(mappedZones.Count / (double)request.PageSize),
            Items = mappedZones
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList()
        };

        logger.LogInformation(
            "Retrieved {ReturnedCount} delivery zones out of {TotalCount} for Branch ID: {BranchId}",
            paginatedResponse.Items.Count,
            paginatedResponse.TotalCount,
            request.BranchId);

        return paginatedResponse;
    }
}