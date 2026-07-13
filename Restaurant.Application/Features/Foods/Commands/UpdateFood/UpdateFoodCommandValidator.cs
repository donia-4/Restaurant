using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Foods.Commands.UpdateFood
{
    public sealed class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
    {
        public UpdateFoodCommandValidator()
        {
            RuleFor(x => x.FoodId)
                .NotEmpty().WithMessage("Food ID is required.");

            When(x => x.Request.Name is not null, () =>
            {
                RuleFor(x => x.Request.Name)
                    .NotEmpty().WithMessage("Food name cannot be empty.");
            });

            When(x => x.Request.Price.HasValue, () =>
            {
                RuleFor(x => x.Request.Price.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");
            });

            When(x => x.Request.PreparationTimeMinutes.HasValue, () =>
            {
                RuleFor(x => x.Request.PreparationTimeMinutes.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Preparation time must be zero or greater.");
            });
        }
    }
}
