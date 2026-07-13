using FluentValidation;

namespace Restaurant.Application.Features.Branches.Commands.UpdateBranch;

public sealed class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    public UpdateBranchCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty();

        RuleFor(x => x.Request.Name)
            .MaximumLength(100)
            .When(x => x.Request.Name is not null);

        RuleFor(x => x.Request.Address)
            .MaximumLength(250)
            .When(x => x.Request.Address is not null);

        RuleFor(x => x.Request.Phone)
            .MaximumLength(20)
            .When(x => x.Request.Phone is not null);

        RuleForEach(x => x.Request.WorkingHours!)
            .ChildRules(hour =>
            {
                hour.RuleFor(x => x.DayOfWeek)
                    .IsInEnum();

                hour.RuleFor(x => x.OpenTime)
                    .LessThan(x => x.CloseTime)
                    .When(x => !x.IsClosed);
            })
            .When(x => x.Request.WorkingHours is not null);
    }
}