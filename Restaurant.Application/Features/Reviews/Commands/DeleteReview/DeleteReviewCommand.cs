using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Reviews.Commands.DeleteReview
{
    public sealed record DeleteReviewCommand(
    Guid ReviewId,
    Guid UserId) : IRequest<Result<Deleted>>;
}
