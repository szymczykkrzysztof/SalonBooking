using SalonBooking.Api.Common;

namespace SalonBooking.Api.Domain.ServiceCatalog;

public static class ServiceErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Service.NotFound", "Service does not exist.");
    public static readonly Error InvalidName =
        Error.BusinessRule("Service.InvalidName", "Name is required and can have at most 100 characters.");
    public static readonly Error InvalidDuration =
        Error.BusinessRule("Service.InvalidDuration", "Duration must be positive and divisible by 15 minutes.");
    public static readonly Error InvalidPrice =
        Error.BusinessRule("Service.InvalidPrice", "Price must be greater than zero.");
}