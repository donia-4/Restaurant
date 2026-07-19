using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Reviews.Dtos.UpdateReview
{
    public sealed record UpdateReviewRequest
    {
        public int? Rating { get; init; }
        public string? Comment { get; init; }
    }
}
