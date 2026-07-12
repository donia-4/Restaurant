using FluentValidation;

namespace Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;

public sealed class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => x.Description is not null);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .When(x => x.Phone is not null);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .When(x => x.Email is not null);

        RuleFor(x => x.Address)
            .NotEmpty()
            .When(x => x.Address is not null);

        RuleFor(x => x.CuisineType)
            .IsInEnum()
            .When(x => x.CuisineType.HasValue);
    }
}