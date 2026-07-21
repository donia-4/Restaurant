using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Branches.Dtos.CreateBranch;
using Restaurant.Domain.Branches;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;
using Restaurant.Domain.WorkingHours;

namespace Restaurant.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandHandler(
    IRestaurantRepository restaurantRepository,
    IBranchRepository branchRepository, ILogger<CreateBranchCommandHandler> logger)
    : IRequestHandler<CreateBranchCommand, Result<CreateBranchResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository = restaurantRepository;
    private readonly IBranchRepository _branchRepository = branchRepository;

    public async Task<Result<CreateBranchResponse>> Handle(
        CreateBranchCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing CreateBranchCommand for Restaurant ID: {RestaurantId}",
            command.Request.RestaurantId);
        
        var request = command.Request;

        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        bool duplicateName = await _branchRepository.ExistsWithTheGivenName(request.Name.ToLower(), cancellationToken);

        if (duplicateName)
            return BranchErrors.DuplicateName;

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

        logger.LogInformation(
            "Successfully created Branch with ID: {BranchId} for Restaurant ID: {RestaurantId}",
            branch.Id,
            command.Request.RestaurantId);

        return new CreateBranchResponse(branch.Id);
    }
}