using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Branches.Dtos.DeactivateBranch;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.DeactivateBranch
{
    public sealed class DeactivateBranchCommandHandler(
    IBranchRepository branchRepository,
    ICacheService cacheService)
    : IRequestHandler<DeactivateBranchCommand, Result<DeactivateBranchResponse>>
    {
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<DeactivateBranchResponse>> Handle(
            DeactivateBranchCommand request,
            CancellationToken cancellationToken)
        {
            var branch = await _branchRepository.GetByIdAsync(
                request.BranchId,
                cancellationToken);

            if (branch is null)
            {
                return BranchErrors.NotFound;
            }

            var deactivateResult = branch.Deactivate();

            if (deactivateResult.IsError)
            {
                return deactivateResult.TopError;
            }

            await _branchRepository.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await _cacheService.RemoveByTagAsync($"branch:{branch.Id}", cancellationToken);
            await _cacheService.RemoveByTagAsync($"restaurant:{branch.RestaurantId}:branches", cancellationToken);
            await _cacheService.RemoveByTagAsync("branches", cancellationToken);

            return new DeactivateBranchResponse(branch.Id);
        }
    }
}
