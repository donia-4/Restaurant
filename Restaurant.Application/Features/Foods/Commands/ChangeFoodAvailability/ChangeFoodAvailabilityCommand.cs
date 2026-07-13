using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.ChangeFoodAvailability
{
    public sealed record ChangeFoodAvailabilityCommand(Guid FoodId, bool IsAvailable) : IRequest<Result<Updated>>;

}
