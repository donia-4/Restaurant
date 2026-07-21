using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Reviews.Commands.UpdateReview
{
    public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .NotEmpty().WithMessage("Review ID is required.");

            RuleFor(x => x.Request.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Request.Rating.HasValue);

            RuleFor(x => x.Request.Comment)
                .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Request.Comment));
        }
    }
}
