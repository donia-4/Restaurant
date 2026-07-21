using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Reviews.Dtos.CreateReview;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Domain.Results;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Commands.CreateReview
{
    public sealed record CreateReviewCommand(
    Guid UserId,
    CreateReviewRequest Request) : IRequest<Result<ReviewResponse>>;
}
