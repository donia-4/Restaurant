using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Reviews.Dtos.CreateReview
{
    public sealed record CreateReviewRequest
    {
        public Guid RestaurantId { get; init; }
        public int Rating { get; init; }
        public string? Comment { get; init; }
    }

}
