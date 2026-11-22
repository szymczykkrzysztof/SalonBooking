using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;

public sealed record GetServicesQuery(ServiceCategory? Category);

public sealed class GetServicesHandler(IServiceRepository services)
{
    public async Task<IReadOnlyList<ServiceResponse>> Handle(GetServicesQuery query, CancellationToken ct)
    {
        var list = await services.GetActiveAsync(query.Category, ct);
        return list.Select(ServiceResponse.From).ToList();
    }
}