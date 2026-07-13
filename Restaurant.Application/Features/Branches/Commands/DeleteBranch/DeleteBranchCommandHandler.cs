using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.DeleteBranch
{
    public sealed class DeleteBranchCommandHandler(
    IBranchRepository branchRepository,
    IRestaurantRepository restaurantRepository,
    ICacheService cacheService)
    : IRequestHandler<DeleteBranchCommand, Result<Deleted>>
    {
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly IRestaurantRepository _restaurantRepository = restaurantRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<Deleted>> Handle(
            DeleteBranchCommand request,
            CancellationToken cancellationToken)
        {
            var branch = await _branchRepository.GetByIdAsync(
                request.BranchId,
                cancellationToken);

            if (branch is null)
            {
                return BranchErrors.NotFound;
            }

            var restaurant = await _restaurantRepository.GetByIdAsync(
                branch.RestaurantId,
                cancellationToken);

            if (restaurant is null)
            {
                return Restaurant.Domain.Restaurants.RestaurantErrors.NotFound;
            }

            var removeBranchResult = restaurant.RemoveBranch(branch);

            if (removeBranchResult.IsError)
            {
                return removeBranchResult.TopError;
            }

            _branchRepository.Remove(branch);

            await _branchRepository.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await _cacheService.RemoveByTagAsync($"branch:{branch.Id}", cancellationToken);
            await _cacheService.RemoveByTagAsync($"restaurant:{branch.RestaurantId}:branches", cancellationToken);
            await _cacheService.RemoveByTagAsync("branches", cancellationToken);

            return Result.Deleted;
        }
    }
}
