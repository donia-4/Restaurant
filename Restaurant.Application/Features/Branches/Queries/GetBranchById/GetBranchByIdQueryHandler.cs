using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetBranchById;

public sealed class GetBranchByIdQueryHandler(
    IBranchRepository branchRepository)
    : IRequestHandler<GetBranchByIdQuery, Result<BranchResponse>>
{
    private readonly IBranchRepository _branchRepository = branchRepository;

    public async Task<Result<BranchResponse>> Handle(
        GetBranchByIdQuery request,
        CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdWithWorkingHoursAsync(
            request.BranchId,
            cancellationToken);

        if (branch is null)
        {
            return BranchErrors.NotFound;
        }

        var response = new BranchResponse(
            branch.Id,
            branch.RestaurantId,
            branch.Name,
            branch.Address,
            branch.Latitude,
            branch.Longitude,
            branch.Phone,
            branch.IsActive,
            branch.WorkingHours.Select(wh => new WorkingHourResponse(
                wh.Id,
                wh.DayOfWeek,
                wh.OpenTime,
                wh.CloseTime,
                wh.IsClosed)).ToList());

        return response;
    }
}
