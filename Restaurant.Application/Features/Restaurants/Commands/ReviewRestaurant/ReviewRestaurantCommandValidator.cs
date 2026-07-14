using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Commands.ReviewRestaurant
{
    public sealed class ReviewRestaurantCommandValidator : AbstractValidator<ReviewRestaurantCommand>
    {
        public ReviewRestaurantCommandValidator()
        {
            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID is required.");

            RuleFor(x => x.Request.Status)
                .Must(status => status is RestaurantStatus.Approved
                             or RestaurantStatus.Rejected
                             or RestaurantStatus.Pending)
                .WithMessage("Status must be Approved, Rejected, or Pending.");

            RuleFor(x => x.Request.Reason)
                .NotEmpty()
                .When(x => x.Request.Status == RestaurantStatus.Rejected)
                .WithMessage("Rejection reason is required when rejecting.");

            RuleFor(x => x.Request.Reason)
                .Empty()
                .When(x => x.Request.Status != RestaurantStatus.Rejected)
                .WithMessage("Reason should only be provided when rejecting.");
        }
    }
}
