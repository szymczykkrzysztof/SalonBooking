using FluentValidation.TestHelper;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.UnitTests.Application;

public class CreateServiceCommandValidatorTests
{
    private readonly CreateServiceCommandValidator _validator = new();

    private static CreateServiceCommand Valid() =>
        new("Strzyżenie damskie", ServiceCategory.Haircut, 60, 120m);
    
    [Fact]
    public void ValidCommand_HasNoErrors()
    {
        _validator.TestValidate(Valid()).ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void NameOf101Characters_HasErrorForName()
    {
        var command = Valid() with { Name = new string('a', 101) };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    public void InvalidDuration_HasErrorForDuration(int minutes)
    {
        var command = Valid() with { DurationMinutes = minutes };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.DurationMinutes);
    }
}