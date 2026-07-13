using FluentValidation;

namespace Restaurant.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandValidator
    : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Request.RestaurantId)
            .NotEmpty();

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.Address)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.Request.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Request.WorkingHours)
            .NotEmpty();

        RuleForEach(x => x.Request.WorkingHours)
            .ChildRules(hour =>
            {
                hour.RuleFor(x => x.DayOfWeek)
                    .IsInEnum();

                hour.RuleFor(x => x.OpenTime)
                    .LessThan(x => x.CloseTime)
                    .When(x => !x.IsClosed);
            });
    }
}