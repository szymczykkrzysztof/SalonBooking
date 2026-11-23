using SalonBooking.Api.Common;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.UnitTests.Domain;

public class ServiceTests
{
    private static readonly Money Price = Money.Create(120m, "PLN").Value;

    private static Result<Service>
        CreateWith(string name = "Strzyżenie damskie", int minutes = 60, Money? price = null) =>
        Service.Create(name, ServiceCategory.Haircut, TimeSpan.FromMinutes(minutes), price ?? Price);

    [Fact]
    public void Create_WithValidData_ReturnsActiveService()
    {
        var result = CreateWith();
        
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsActive);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }
    
    [Theory]
    [InlineData(15)]
    [InlineData(60)]
    [InlineData(135)]
    public void Create_WithDurationDivisibleBy15_Succeeds(int minutes)
    {
        Assert.True(CreateWith(minutes: minutes).IsSuccess);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-15)]
    [InlineData(20)]
    public void Create_WithInvalidDuration_Fails(int minutes)
    {
        Assert.Equal(ServiceErrors.InvalidDuration, CreateWith(minutes: minutes).Error);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_Fails(string name)
    {
        Assert.Equal(ServiceErrors.InvalidName, CreateWith(name: name).Error);
    }
    
    [Fact]
    public void Create_WithNameOf100Characters_Succeeds()
    {
        Assert.True(CreateWith(name: new string('a', 100)).IsSuccess);
    }

    [Fact]
    public void Create_WithNameOf101Characters_Fails()
    {
        Assert.Equal(ServiceErrors.InvalidName, CreateWith(name: new string('a', 101)).Error);
    }

    [Fact]
    public void Create_WithZeroPrice_Fails()
    {
        var zero = Money.Create(0m, "PLN").Value;

        Assert.Equal(ServiceErrors.InvalidPrice, CreateWith(price: zero).Error);
    }

    [Fact]
    public void Update_WithValidData_ChangesFields()
    {
        // Arrange
        var service = CreateWith().Value;
        var newPrice = Money.Create(150m, "PLN").Value;
        // Act
        var result = service.Update("Strzyżenie męskie", ServiceCategory.Haircut, TimeSpan.FromMinutes(30), newPrice);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Strzyżenie męskie", service.Name);
        Assert.Equal(TimeSpan.FromMinutes(30), service.DefaultDuration);
        Assert.Equal(150m, service.DefaultPrice.Amount);
    }
    [Fact]
    public void Update_WithInvalidDuration_FailsAndKeepsOldValues()
    {
        // Arrange
        var service = CreateWith(name: "Strzyżenie damskie", minutes: 60).Value;

        // Act
        var result = service.Update("Nowa nazwa", ServiceCategory.Haircut, TimeSpan.FromMinutes(20), Price);

        // Assert
        Assert.Equal(ServiceErrors.InvalidDuration, result.Error);
        Assert.Equal("Strzyżenie damskie", service.Name);       
        Assert.Equal(TimeSpan.FromMinutes(60), service.DefaultDuration);
    }
}