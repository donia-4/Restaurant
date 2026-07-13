using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Branches.Commands.UpdateBranch
{
    public sealed class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(x => x.Request.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

            RuleFor(x => x.Request.Address)
                .NotEmpty().WithMessage("Branch address is required.");

            RuleFor(x => x.Request.Phone)
                .NotEmpty().WithMessage("Branch phone is required.");

            RuleFor(x => x.Request.WorkingHours)
                .NotNull().WithMessage("Working hours are required.");
        }
    }
}
