using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.AddOns.Commands.DeleteAddOn;
using Restaurant.Application.Features.AddOns.Dtos.UpdateAddOn;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Commands.UpdateAddOn
{
    public sealed class UpdateAddOnCommandHandler(
    IAddOnRepository addOnRepository, ILogger<UpdateAddOnCommandHandler> logger)
    : IRequestHandler<UpdateAddOnCommand, Result<UpdateAddOnResponse>>
    {
        public async Task<Result<UpdateAddOnResponse>> Handle(
            UpdateAddOnCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Processing UpdateAddOnCommand for AddOn ID: {AddOnId}",
                command.AddOnId);

            var addOn = await addOnRepository.GetByIdAsync(
                command.AddOnId,
                cancellationToken);

            if (addOn is null)
                return AddOnErrors.NotFound;

            // Partial update - only update provided fields
            var name = command.Request.Name ?? addOn.Name;
            var price = command.Request.Price ?? addOn.Price;
            var isRequired = command.Request.IsRequired ?? addOn.IsRequired;
            var maxQuantity = command.Request.MaxQuantity ?? addOn.MaxQuantity;

            var updateResult = addOn.Update(name, price, isRequired, maxQuantity);
            if (updateResult.IsError)
                return updateResult.Errors;

            await addOnRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Successfully updated AddOn with ID: {AddOnId}",
                command.AddOnId);

            return new UpdateAddOnResponse(
                addOn.Id,
                addOn.Name,
                addOn.Price,
                addOn.IsRequired,
                addOn.MaxQuantity);
        }
    }

}
