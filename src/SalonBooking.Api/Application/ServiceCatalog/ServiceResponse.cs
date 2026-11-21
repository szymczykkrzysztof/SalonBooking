using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;

public sealed record ServiceResponse(
    Guid Id,
    string Name,
    ServiceCategory Category,
    int DurationMinutes,
    decimal Price,
    string Currency)
{
    public static ServiceResponse From(Service service) => new(
        service.Id,
        service.Name,
        service.Category,
        (int) service.DefaultDuration.TotalMinutes,
        service.DefaultPrice.Amount,
        service.DefaultPrice.Currency);
}