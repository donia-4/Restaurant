using Restaurant.Domain.Categories;
using Restaurant.Domain.Common;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;
using Restaurant.Domian.Restaurants.Enums;

namespace Restaurant.Domain.Restaurants;

public sealed class Restaurant : AuditableEntity
{
    private readonly List<Food> _foods = [];
    private readonly List<Category> _categories = [];


    public Guid OwnerId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string? Logo { get; private set; }

    public RestaurantStatus Status { get; private set; }

    public IReadOnlyCollection<Food> Foods => _foods;
    public IReadOnlyCollection<Category> Categories => _categories;

    private Restaurant()
    {
    }

    private Restaurant(
        Guid id,
        Guid ownerId,
        string name,
        string description,
        string address,
        string phone,
        string? logo)
        : base(id)
    {
        OwnerId = ownerId;
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Logo = logo;
        Status = RestaurantStatus.Pending;
    }

    public static Result<Restaurant> Create(
        Guid id,
        Guid ownerId,
        string name,
        string description,
        string address,
        string phone,
        string? logo = null)
    {
        if (ownerId == Guid.Empty)
            return RestaurantErrors.InvalidOwner;

        if (string.IsNullOrWhiteSpace(name))
            return RestaurantErrors.InvalidName;

        if (string.IsNullOrWhiteSpace(address))
            return RestaurantErrors.InvalidAddress;

        if (string.IsNullOrWhiteSpace(phone))
            return RestaurantErrors.InvalidPhone;

        return new Restaurant(
            id,
            ownerId,
            name.Trim(),
            description.Trim(),
            address.Trim(),
            phone.Trim(),
            logo);
    }

    public Result<Updated> Approve()
    {
        Status = RestaurantStatus.Approved;
        return Result.Updated;
    }

    public Result<Updated> Reject()
    {
        Status = RestaurantStatus.Rejected;
        return Result.Updated;
    }

    public Result<Updated> Close()
    {
        Status = RestaurantStatus.Closed;
        return Result.Updated;
    }
}