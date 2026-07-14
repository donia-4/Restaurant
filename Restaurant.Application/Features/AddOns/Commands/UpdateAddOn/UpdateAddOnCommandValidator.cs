using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.AddOns.Commands.UpdateAddOn
{
    public sealed class UpdateAddOnCommandValidator : AbstractValidator<UpdateAddOnCommand>
    {
        public UpdateAddOnCommandValidator()
        {
            RuleFor(x => x.AddOnId)
                .NotEmpty().WithMessage("Add-on ID is required.");

            When(x => x.Request.Name is not null, () =>
            {
                RuleFor(x => x.Request.Name)
                    .NotEmpty().WithMessage("Add-on name cannot be empty.")
                    .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            });

            When(x => x.Request.Price.HasValue, () =>
            {
                RuleFor(x => x.Request.Price!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");
            });

            When(x => x.Request.MaxQuantity.HasValue, () =>
            {
                RuleFor(x => x.Request.MaxQuantity!.Value)
                    .GreaterThan(0).WithMessage("Max quantity must be greater than 0.");
            });
        }
    }
}
