using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Application.Features.Reviews.Dtos.UpdateReview;
using Restaurant.Domain.Results;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Commands.UpdateReview
{
    public sealed record UpdateReviewCommand(
    Guid ReviewId,
    Guid UserId,
    UpdateReviewRequest Request) : IRequest<Result<ReviewResponse>>;
}
