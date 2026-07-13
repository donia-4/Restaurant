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
            RuleFor(x => x.FoodId).NotEmpty();
            RuleFor(x => x.Request.Name).NotEmpty();
            RuleFor(x => x.Request.CategoryId).NotEmpty();
            RuleFor(x => x.Request.Price).GreaterThanOrEqualTo(0);
        }
    }
}
