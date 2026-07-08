using Restaurant.Domain.Results;

namespace Restaurant.Domain.Restaurants;

public static class RestaurantErrors
{
    public static readonly Error InvalidOwner = Error.Validation("Restaurant.Owner.Invalid", "Restaurant owner is required.");

    public static readonly Error InvalidName = Error.Validation("Restaurant.Name.Required", "Restaurant name is required.");

    public static readonly Error InvalidAddress = Error.Validation("Restaurant.Address.Required", "Restaurant address is required.");

    public static readonly Error InvalidPhone = Error.Validation("Restaurant.Phone.Required", "Restaurant phone is required.");

    public static readonly Error AlreadyApproved = Error.Conflict("Restaurant.AlreadyApproved", "Restaurant is already approved.");

    public static readonly Error NotFound = Error.NotFound("Restaurant.NotFound", "Restaurant was not found.");
}