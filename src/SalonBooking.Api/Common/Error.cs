namespace SalonBooking.Api.Common;

public enum ErrorType { Validation, NotFound, Conflict, BusinessRule, Forbidden }
public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Validation);

    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
    public static Error BusinessRule(string code, string description) => new(code, description, ErrorType.BusinessRule);
}