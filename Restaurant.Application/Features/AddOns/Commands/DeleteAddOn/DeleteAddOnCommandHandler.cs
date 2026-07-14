using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Commands.DeleteAddOn
{
    public sealed class DeleteAddOnCommandHandler(
    IAddOnRepository addOnRepository)
    : IRequestHandler<DeleteAddOnCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(
            DeleteAddOnCommand command,
            CancellationToken cancellationToken)
        {
            var addOn = await addOnRepository.GetByIdAsync(
                command.AddOnId,
                cancellationToken);

            if (addOn is null)
                return AddOnErrors.NotFound;

            addOnRepository.Remove(addOn);
            await addOnRepository.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }

}
