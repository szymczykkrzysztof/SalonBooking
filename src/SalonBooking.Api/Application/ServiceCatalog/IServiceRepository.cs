using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;

public interface IServiceRepository
{
    void Add(Service service);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct);
}