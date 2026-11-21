using SalonBooking.Api.Common;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;

public sealed record GetServiceByIdQuery(Guid Id);

public sealed class GetServiceByIdHandler(IServiceRepository services)
{
    public async Task<Result<ServiceResponse>> Handle(GetServiceByIdQuery query, CancellationToken ct)
    {
        var service = await services.GetByIdAsync(query.Id, ct);

        return service is null
            ? Result.Failure<ServiceResponse>(ServiceErrors.NotFound)
            : Result.Success(ServiceResponse.From(service));
    }
}