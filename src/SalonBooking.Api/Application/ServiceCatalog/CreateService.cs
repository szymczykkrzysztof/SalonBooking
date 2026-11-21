using FluentValidation;
using SalonBooking.Api.Application.Abstractions;
using SalonBooking.Api.Common;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Application.ServiceCatalog;
public sealed record CreateServiceCommand(string Name, ServiceCategory Category, int DurationMinutes, decimal Price);
public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(Service.NameMaxLength);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.DurationMinutes).GreaterThan(0)
            .Must(m => m % 15 == 0).WithMessage("Duration must be a multiple of 15 minutes.");
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public sealed class CreateServiceHandler(IServiceRepository services, IUnitOfWork unitOfWork)
{
    public async Task<Result<Guid>> Handle(CreateServiceCommand command, CancellationToken ct)
    {
        var price = Money.Create(command.Price, "PLN");
        if (price.IsFailure)
            return Result.Failure<Guid>(price.Error);

        var service = Service.Create(command.Name, command.Category,
            TimeSpan.FromMinutes(command.DurationMinutes), price.Value);
        if (service.IsFailure)
            return Result.Failure<Guid>(service.Error);

        services.Add(service.Value);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(service.Value.Id);
    }
}