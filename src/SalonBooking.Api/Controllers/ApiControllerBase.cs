using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.Common;

namespace SalonBooking.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleFailure(Error error) =>
        Problem(statusCode: ToStatusCode(error.Type), title: error.Code, detail: error.Description);

    protected IActionResult ValidationFailed(ValidationResult validation) =>
        ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));

    private static int ToStatusCode(ErrorType type) => type switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.BusinessRule => StatusCodes.Status422UnprocessableEntity,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status400BadRequest
    };
}