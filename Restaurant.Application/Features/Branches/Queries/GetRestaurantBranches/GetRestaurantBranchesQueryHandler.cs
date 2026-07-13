using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches
{
    public sealed class GetRestaurantBranchesQueryHandler(
    IBranchRepository branchRepository)
    : IRequestHandler<GetRestaurantBranchesQuery, Result<IReadOnlyList<BranchResponse>>>
    {
        private readonly IBranchRepository _branchRepository = branchRepository;

        public async Task<Result<IReadOnlyList<BranchResponse>>> Handle(
            GetRestaurantBranchesQuery request,
            CancellationToken cancellationToken)
        {
            var branches = await _branchRepository.GetByRestaurantIdAsync(
                request.RestaurantId,
                cancellationToken);

            var response = branches.Select(branch => new BranchResponse(
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
                    wh.IsClosed)).ToList())).ToList();

            return response;
        }
    }
}
