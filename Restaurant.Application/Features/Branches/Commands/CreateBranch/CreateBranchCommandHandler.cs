using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Branches.Dtos.CreateBranch;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;
using Restaurant.Domain.WorkingHours;

namespace Restaurant.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandHandler(
    IRestaurantRepository restaurantRepository,
    IBranchRepository branchRepository)
    : IRequestHandler<CreateBranchCommand, Result<CreateBranchResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository = restaurantRepository;
    private readonly IBranchRepository _branchRepository = branchRepository;

    public async Task<Result<CreateBranchResponse>> Handle(
        CreateBranchCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        var branchResult = Branch.Create(
            Guid.NewGuid(),
            request.RestaurantId,
            request.Name,
            request.Address,
            request.Latitude,
            request.Longitude,
            request.Phone);

        if (branchResult.IsError)
        {
            return branchResult.TopError;
        }

        var branch = branchResult.Value;

        var workingHours = new List<WorkingHour>();

        foreach (var item in request.WorkingHours)
        {
            var workingHourResult = WorkingHour.Create(
                Guid.NewGuid(),
                branch.Id,
                item.DayOfWeek,
                item.OpenTime,
                item.CloseTime,
                item.IsClosed);

            if (workingHourResult.IsError)
            {
                return workingHourResult.TopError;
            }

            workingHours.Add(workingHourResult.Value);
        }

        var replaceWorkingHoursResult = branch.ReplaceWorkingHours(workingHours);

        if (replaceWorkingHoursResult.IsError)
        {
            return replaceWorkingHoursResult.TopError;
        }

        var addBranchResult = restaurant.AddBranch(branch);

        if (addBranchResult.IsError)
        {
            return addBranchResult.TopError;
        }

        await _branchRepository.AddAsync(
            branch,
            cancellationToken);

        await _branchRepository.SaveChangesAsync(
            cancellationToken);

        return new CreateBranchResponse(branch.Id);
    }
}