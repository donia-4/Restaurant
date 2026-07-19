using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Reviews.Commands.CreateReview
{
    public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Request.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID is required.");

            RuleFor(x => x.Request.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Request.Comment)
                .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Request.Comment));
        }
    }
}
