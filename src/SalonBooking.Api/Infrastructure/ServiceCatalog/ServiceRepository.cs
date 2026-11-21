using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Infrastructure.ServiceCatalog;

internal sealed class ServiceRepository(AppDbContext db) : IServiceRepository
{
    public void Add(Service service) => db.Services.Add(service);

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Services.FirstOrDefaultAsync(s => s.Id == id && s.IsActive, ct);
}