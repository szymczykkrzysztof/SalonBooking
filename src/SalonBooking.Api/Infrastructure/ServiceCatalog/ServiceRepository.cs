using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Infrastructure.ServiceCatalog;

internal sealed class ServiceRepository(AppDbContext db) : IServiceRepository
{
    public void Add(Service service) => db.Services.Add(service);

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Services.FirstOrDefaultAsync(s => s.Id == id && s.IsActive, ct);

    public async Task<IReadOnlyList<Service>> GetActiveAsync(ServiceCategory? category, CancellationToken ct)
    {
        var query = db.Services.AsNoTracking().Where(s => s.IsActive);
        if (category is not null)
            query = query.Where(s => s.Category == category);

        return await query.OrderBy(s => s.Name).ToListAsync(ct);
    }
}