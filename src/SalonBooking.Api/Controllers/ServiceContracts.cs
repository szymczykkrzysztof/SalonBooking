using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Controllers;

public sealed record CreateServiceRequest(string Name, ServiceCategory Category, int DurationMinutes, decimal Price);
public sealed record CreateServiceResponse(Guid Id);