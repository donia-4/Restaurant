using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Reviews.Dtos.GetReview
{
    public sealed record ReviewResponse(
    Guid Id,
    Guid RestaurantId,
    Guid UserId,
    int Rating,
    string? Comment,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset LastModifiedUtc);
}
