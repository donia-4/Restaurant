using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Foods.Commands.CreateFood
{
    public sealed class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
    {
        public CreateFoodCommandValidator()
        {
            RuleFor(x => x.Request.RestaurantId).NotEmpty();
            RuleFor(x => x.Request.CategoryId).NotEmpty();
            RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Request.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Request.PreparationTimeMinutes).GreaterThanOrEqualTo(0);
        }
    }
}
