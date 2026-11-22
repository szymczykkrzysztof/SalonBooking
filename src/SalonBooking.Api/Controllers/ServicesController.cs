using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Controllers;

[Route("api/services")]
public sealed class ServicesController : ApiControllerBase{
    [HttpPost]
    [ProducesResponseType<CreateServiceResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateServiceRequest request,
        [FromServices] IValidator<CreateServiceCommand> validator,
        [FromServices] CreateServiceHandler handler,
        CancellationToken ct)
    {
        var command = new CreateServiceCommand(request.Name, request.Category, request.DurationMinutes, request.Price);

        var validation = await validator.ValidateAsync(command, ct);
        if (!validation.IsValid)
            return ValidationFailed(validation);

        var result = await handler.Handle(command, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new CreateServiceResponse(result.Value))
            : HandleFailure(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ServiceResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetServiceByIdHandler handler, CancellationToken ct)
    {
        var result = await handler.Handle(new GetServiceByIdQuery(id), ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result.Error);
    }
    
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ServiceResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] ServiceCategory? category,
        [FromServices] GetServicesHandler handler,
        CancellationToken ct)
    {
        var services = await handler.Handle(new GetServicesQuery(category), ct);
        return Ok(services);
    }
}