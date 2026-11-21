using System.Net;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests;
[Collection(nameof(ApiCollection))]
public class HealthTests(SalonBookingApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_Health_Returns200AndHealthy()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}