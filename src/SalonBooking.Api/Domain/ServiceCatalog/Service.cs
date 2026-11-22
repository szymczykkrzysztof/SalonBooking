using SalonBooking.Api.Common;

namespace SalonBooking.Api.Domain.ServiceCatalog;

public sealed class Service : Entity
{
    public const int NameMaxLength = 100;

    private Service()
    {
    }

    public string Name { get; private set; } = null!;
    public ServiceCategory Category { get; private set; }
    public TimeSpan DefaultDuration { get; private set; }
    public Money DefaultPrice { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public static Result<Service> Create(string name, ServiceCategory category, TimeSpan duration, Money price)
    {
        var error = Validate(name, duration, price);
        if (error is not null)
            return Result.Failure<Service>(error);

        return Result.Success(new Service
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Category = category,
            DefaultDuration = duration,
            DefaultPrice = price,
            IsActive = true
        });
    }

    private static Error? Validate(string name, TimeSpan duration, Money price)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > NameMaxLength)
            return ServiceErrors.InvalidName;
        if (duration <= TimeSpan.Zero || duration.TotalMinutes % 15 != 0)
            return ServiceErrors.InvalidDuration;
        if (price.Amount <= 0)
            return ServiceErrors.InvalidPrice;
        return null;
    }

    public Result Update(string name, ServiceCategory category, TimeSpan duration, Money price)
    {
        var error = Validate(name, duration, price);
        if (error is not null)
            return Result.Failure(error);

        Name = name.Trim();
        Category = category;
        DefaultDuration = duration;
        DefaultPrice = price;

        return Result.Success();
    }
}