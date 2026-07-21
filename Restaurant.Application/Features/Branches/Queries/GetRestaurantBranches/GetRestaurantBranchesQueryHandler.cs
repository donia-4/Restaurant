using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches;

public sealed class GetRestaurantBranchesQueryHandler(
    IBranchRepository branchRepository,
    ILogger<GetRestaurantBranchesQueryHandler> logger)
    : IRequestHandler<GetRestaurantBranchesQuery, Result<PaginatedList<BranchResponse>>>
{
    public async Task<Result<PaginatedList<BranchResponse>>> Handle(
        GetRestaurantBranchesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving branches for Restaurant {RestaurantId}. PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.RestaurantId,
            request.PageNumber,
            request.PageSize);

        var query = branchRepository
            .GetByRestaurantId(request.RestaurantId)
            .Select(branch => new BranchResponse(
                branch.Id,
                branch.RestaurantId,
                branch.Name,
                branch.Address,
                branch.Latitude,
                branch.Longitude,
                branch.Phone,
                branch.IsActive,
                branch.WorkingHours
                    .Select(wh => new WorkingHourResponse(
                        wh.Id,
                        wh.DayOfWeek,
                        wh.OpenTime,
                        wh.CloseTime,
                        wh.IsClosed))
                    .ToList()));

        var paginatedBranches =
            await PaginatedList<BranchResponse>.CreateAsync(
                query,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

        logger.LogInformation(
            "Retrieved {ReturnedCount} branches out of {TotalCount} for Restaurant {RestaurantId}",
            paginatedBranches.Items.Count,
            paginatedBranches.TotalCount,
            request.RestaurantId);

        return paginatedBranches;
    }
}