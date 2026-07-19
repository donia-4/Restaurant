using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Dtos.Mappings
{
    public static class ReviewMappings
    {
        public static ReviewResponse ToResponse(this Review review)
        {
            return new ReviewResponse(
                review.Id,
                review.RestaurantId,
                review.UserId,
                review.Rating,
                review.Comment,
                review.CreatedAtUtc,
                review.LastModifiedUtc);
        }
    }
}
