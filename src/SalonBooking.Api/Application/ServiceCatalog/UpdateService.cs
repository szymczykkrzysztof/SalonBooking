using FluentValidation;
using SalonBooking.Api.Application.Abstractions;
using SalonBooking.Api.Common;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;

public sealed record UpdateServiceCommand(
    Guid Id,
    string Name,
    ServiceCategory Category,
    int DurationMinutes,
    decimal Price);

public sealed class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(Service.NameMaxLength);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.DurationMinutes).GreaterThan(0)
            .Must(m => m % 15 == 0).WithMessage("Duration must be a multiple of 15 minutes.");
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public sealed class UpdateServiceHandler(IServiceRepository services, IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(UpdateServiceCommand command, CancellationToken ct)
    {
        var service = await services.GetByIdAsync(command.Id, ct);
        if (service is null)
            return Result.Failure(ServiceErrors.NotFound);
        var price = Money.Create(command.Price, "PLN");
        if (price.IsFailure)
            return Result.Failure(price.Error);

        var result = service.Update(command.Name, command.Category,
            TimeSpan.FromMinutes(command.DurationMinutes), price.Value);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}