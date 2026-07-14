using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.AddOns.Dtos.CreateAddOn;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Commands.CreateAddOn
{
    public sealed class CreateAddOnCommandHandler(
    IFoodRepository foodRepository,
    IAddOnRepository addOnRepository)
    : IRequestHandler<CreateAddOnCommand, Result<CreateAddOnResponse>>
    {
        public async Task<Result<CreateAddOnResponse>> Handle(
            CreateAddOnCommand command,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            var food = await foodRepository.GetByIdWithAddOnsAsync(
                request.FoodId,
                cancellationToken);

            if (food is null)
                return FoodErrors.NotFound;

            var addOnResult = AddOn.Create(
                Guid.NewGuid(),
                request.FoodId,
                request.Name,
                request.Price,
                request.IsRequired,
                request.MaxQuantity);

            if (addOnResult.IsError)
                return addOnResult.Errors;

            var addOn = addOnResult.Value;

            // Add to DbContext directly via repository (fixes DbConcurrency)
            await addOnRepository.AddAsync(addOn, cancellationToken);
            await addOnRepository.SaveChangesAsync(cancellationToken);

            return new CreateAddOnResponse(addOn.Id);
        }
    }
}
