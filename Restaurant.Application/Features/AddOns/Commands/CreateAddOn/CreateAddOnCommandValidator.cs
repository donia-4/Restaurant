using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.AddOns.Commands.CreateAddOn
{
    public sealed class CreateAddOnCommandValidator : AbstractValidator<CreateAddOnCommand>
    {
        public CreateAddOnCommandValidator()
        {
            RuleFor(x => x.Request.FoodId)
                .NotEmpty().WithMessage("Food ID is required.");

            RuleFor(x => x.Request.Name)
                .NotEmpty().WithMessage("Add-on name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Request.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");

            RuleFor(x => x.Request.MaxQuantity)
                .GreaterThan(0).When(x => x.Request.MaxQuantity.HasValue)
                .WithMessage("Max quantity must be greater than 0.");
        }
    }

}
