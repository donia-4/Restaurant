using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Branches.Dtos.ActivateBranch;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.ActivateBranch
{
    public sealed class ActivateBranchCommandHandler(
    IBranchRepository branchRepository,
    ICacheService cacheService)
    : IRequestHandler<ActivateBranchCommand, Result<ActivateBranchResponse>>
    {
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<ActivateBranchResponse>> Handle(
            ActivateBranchCommand request,
            CancellationToken cancellationToken)
        {
            var branch = await _branchRepository.GetByIdAsync(
                request.BranchId,
                cancellationToken);

            if (branch is null)
            {
                return BranchErrors.NotFound;
            }

            var activateResult = branch.Activate();

            if (activateResult.IsError)
            {
                return activateResult.TopError;
            }

            await _branchRepository.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await _cacheService.RemoveByTagAsync($"branch:{branch.Id}", cancellationToken);
            await _cacheService.RemoveByTagAsync($"restaurant:{branch.RestaurantId}:branches", cancellationToken);
            await _cacheService.RemoveByTagAsync("branches", cancellationToken);

            return new ActivateBranchResponse(branch.Id);
        }
    }
}
