using System.Runtime.InteropServices;
using SalonBooking.Api.Common;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.UnitTests.Domain;

public class MoneyTests
{
    [Fact]
    public void Create_WithPositiveAmount_Succeeds()
    {
        var result = Money.Create(120m,"PLN");
        
        Assert.True(result.IsSuccess);
        Assert.Equal(120m, result.Value.Amount);
    }
    
    [Fact]
    public void Create_WithNegativeAmount_Fails()
    {
        var result = Money.Create(-120m,"PLN");

        Assert.Equal(MoneyErrors.Negative, result.Error);
    }
}