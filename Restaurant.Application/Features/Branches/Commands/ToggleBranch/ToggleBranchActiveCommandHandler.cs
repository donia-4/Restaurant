using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Branches.Commands.ToggleBranch;
using Restaurant.Application.Features.Branches.Dtos.DeactivateBranch;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.DeactivateBranch
{
    public sealed class ToggleBranchActiveCommandHandler(
    IBranchRepository branchRepository,
    ICacheService cacheService)
    : IRequestHandler<ToggleBranchActiveCommand, Result<ToggleBranchActiveResponse>>
    {
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<ToggleBranchActiveResponse>> Handle(
            ToggleBranchActiveCommand request,
            CancellationToken cancellationToken)
        {
            var branch = await _branchRepository.GetByIdAsync(
                request.BranchId,
                cancellationToken);

            if (branch is null)
            {
                return BranchErrors.NotFound;
            }

            Result<Updated> toggleResult;

            if (branch.IsActive)
            {
                toggleResult = branch.Deactivate();
            }
            else
            {
                toggleResult = branch.Activate();
            }

            if (toggleResult.IsError)
            {
                return toggleResult.TopError;
            }

            await _branchRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveByTagAsync($"branch:{branch.Id}", cancellationToken);
            await _cacheService.RemoveByTagAsync($"restaurant:{branch.RestaurantId}:branches", cancellationToken);
            await _cacheService.RemoveByTagAsync("branches", cancellationToken);

            return new ToggleBranchActiveResponse(branch.Id, branch.IsActive);
        }
    }
}
