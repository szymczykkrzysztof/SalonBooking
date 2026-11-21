using SalonBooking.Api.Common;

namespace SalonBooking.Api.Domain.ServiceCatalog;

public sealed record Money(decimal Amount, string Currency)
{
    public static Result<Money> Create(decimal amount, string currency) =>
        amount < 0
            ? Result.Failure<Money>(MoneyErrors.Negative)
            : Result.Success(new Money(amount, currency));
}

public static class MoneyErrors
{
    public static readonly Error Negative =
        Error.BusinessRule("Money.Negative", "Amount cannot be negative.");
}
