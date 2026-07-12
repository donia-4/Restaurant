using FluentValidation;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;

namespace Restaurant.Application.Features.Restaurants.CreateRestaurant;

public sealed class CreateRestaurantCommandValidator
    : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Request.OwnerId)
            .NotEmpty();

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Request.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Request.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.CuisineType)
            .IsInEnum();

        RuleFor(x => x.Request.Address)
            .NotEmpty()
            .MaximumLength(300);
    }
}