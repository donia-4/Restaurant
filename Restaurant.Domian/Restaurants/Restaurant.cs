using Restaurant.Domain.Branches;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Common;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Restaurants.Events;
using Restaurant.Domain.Results;
using Restaurant.Domain.WorkingHours;

namespace Restaurant.Domain.Restaurants;

public sealed class Restaurant : AuditableEntity
{
    private readonly List<Branch> _branches = [];
    private readonly List<Category> _categories = [];
    private readonly List<Food> _foods = [];
    private readonly List<RestaurantImage> _images = [];
    private readonly List<WorkingHour> _workingHours = [];
    private readonly List<DeliveryZone> _deliveryZones = [];

    // FR-01: بيانات المطعم الأساسية
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? Logo { get; private set; }
    public string? CoverImage { get; private set; }
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public CuisineType CuisineType { get; private set; }
    public string Address { get; private set; } = string.Empty;

    // FR-09: حالة المطعم
    public RestaurantStatus Status { get; private set; }
    public bool IsApproved { get; private set; }

    // FR-14: التقييمات (تتحدث من Review Service)
    public decimal AverageRating { get; private set; }
    public int TotalReviews { get; private set; }

    // Navigation Properties
    public IReadOnlyCollection<Branch> Branches => _branches;
    public IReadOnlyCollection<Category> Categories => _categories;
    public IReadOnlyCollection<Food> Foods => _foods;
    public IReadOnlyCollection<RestaurantImage> Images => _images;
    public IReadOnlyCollection<WorkingHour> WorkingHours => _workingHours;
    public IReadOnlyCollection<DeliveryZone> DeliveryZones => _deliveryZones;

    private Restaurant() { }

    private Restaurant(Guid id, Guid ownerId, string name, string description, string phone,
        string email, CuisineType cuisineType, string address, string? logo, string? coverImage)
        : base(id)
    {
        OwnerId = ownerId;
        Name = name;
        Description = description;
        Phone = phone;
        Email = email;
        CuisineType = cuisineType;
        Address = address;
        Logo = logo;
        CoverImage = coverImage;
        Status = RestaurantStatus.Pending;
    }

    // FR-01: إنشاء مطعم
    public static Result<Restaurant> Create(Guid id, Guid ownerId, string name, string description,
        string phone, string email, CuisineType cuisineType, string address,
        string? logo = null, string? coverImage = null)
    {
        if (ownerId == Guid.Empty) return RestaurantErrors.InvalidOwner;
        if (string.IsNullOrWhiteSpace(name)) return RestaurantErrors.InvalidName; // BR-01
        if (string.IsNullOrWhiteSpace(phone)) return RestaurantErrors.InvalidPhone;
        if (string.IsNullOrWhiteSpace(email)) return RestaurantErrors.InvalidEmail;
        if (string.IsNullOrWhiteSpace(address)) return RestaurantErrors.InvalidAddress;

        var restaurant = new Restaurant(id,ownerId,name.Trim(),description.Trim(),phone.Trim(),email.Trim(),cuisineType,address.Trim(),logo,coverImage);

        restaurant.AddDomainEvent(
            new RestaurantRequestedEvent(
                restaurant.Id,
                restaurant.OwnerId,
                restaurant.Name));

        return restaurant;
    }

    // FR-03: تعديل البيانات
    public Result<Updated> UpdateDetails(string name, string description, string phone,
        string email, CuisineType cuisineType, string address, string? logo, string? coverImage)
    {
        if (string.IsNullOrWhiteSpace(name)) return RestaurantErrors.InvalidName;
        if (string.IsNullOrWhiteSpace(phone)) return RestaurantErrors.InvalidPhone;
        if (string.IsNullOrWhiteSpace(email)) return RestaurantErrors.InvalidEmail;
        if (string.IsNullOrWhiteSpace(address)) return RestaurantErrors.InvalidAddress;

        Name = name.Trim(); Description = description.Trim(); Phone = phone.Trim();
        Email = email.Trim(); CuisineType = cuisineType; Address = address.Trim();
        Logo = logo; CoverImage = coverImage;
        return Result.Updated;
    }

    // FR-02: اعتماد/رفض الأدمن
    public Result<Updated> Approve()
    {
        if (IsApproved) return RestaurantErrors.AlreadyApproved;
        Status = RestaurantStatus.Approved; IsApproved = true;
        return Result.Updated;
    }

    public Result<Updated> Reject()
    {
        Status = RestaurantStatus.Rejected; IsApproved = false;
        return Result.Updated;
    }

    public Result<Updated> Suspend()
    {
        Status = RestaurantStatus.Suspended;
        return Result.Updated;
    }

    // FR-09: إدارة الحالة
    public Result<Updated> Open()
    {
        if (!IsApproved) return RestaurantErrors.NotApproved;
        Status = RestaurantStatus.Open;
        return Result.Updated;
    }

    public Result<Updated> Close()
    {
        Status = RestaurantStatus.Closed;
        return Result.Updated;
    }

    public Result<Updated> SetTemporarilyClosed()
    {
        Status = RestaurantStatus.TemporarilyClosed;
        return Result.Updated;
    }

    public Result<Updated> SetUnderMaintenance()
    {
        Status = RestaurantStatus.UnderMaintenance;
        return Result.Updated;
    }

    // FR-14: تحديث التقييم (من Review Service)
    public Result<Updated> UpdateRating(decimal averageRating, int totalReviews)
    {
        AverageRating = averageRating;
        TotalReviews = totalReviews;
        return Result.Updated;
    }

    // FR-04: إدارة الفروع
    public Result<Updated> AddBranch(Branch branch) { _branches.Add(branch); return Result.Updated; }
    public Result<Updated> RemoveBranch(Branch branch)
    {
        if (_branches.Count <= 1) return RestaurantErrors.CannotRemoveLastBranch; // BR-08
        _branches.Remove(branch); return Result.Updated;
    }

    // FR-05: إدارة التصنيفات
    public Result<Updated> AddCategory(Category category) { _categories.Add(category); return Result.Updated; }

    // FR-06: إدارة الأصناف
    public Result<Updated> AddFood(Food food) { _foods.Add(food); return Result.Updated; }

    // FR-11: إدارة الصور
    public Result<Updated> AddImage(RestaurantImage image) { _images.Add(image); return Result.Updated; }
    public Result<Updated> RemoveImage(RestaurantImage image) { _images.Remove(image); return Result.Updated; }

    // FR-10: مناطق التوصيل
    public Result<Updated> AddDeliveryZone(DeliveryZone zone) { _deliveryZones.Add(zone); return Result.Updated; }
    public Result<Updated> RemoveDeliveryZone(DeliveryZone zone) { _deliveryZones.Remove(zone); return Result.Updated; }
}