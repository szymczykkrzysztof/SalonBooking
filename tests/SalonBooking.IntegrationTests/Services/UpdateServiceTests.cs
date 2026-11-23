using System.Net;
using System.Net.Http.Json;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests.Services;

[Collection(nameof(ApiCollection))]
public class UpdateServiceTests(SalonBookingApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task Put_WithValidData_Returns204AndUpdatesService()
    {
        // Arrange
        var id = await _client.CreateServiceAsync(name: "Strzyżenie", durationMinutes: 60, price: 120m);
        var request = new { name = "Strzyżenie premium", category = "Styling", durationMinutes = 90, price = 200m };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/services/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Assert
        var updated = await _client.GetFromJsonAsync<ServiceResponse>($"/api/services/{id}", TestJson.Options);
        Assert.Equal("Strzyżenie premium", updated!.Name);
        Assert.Equal(ServiceCategory.Styling, updated.Category);
        Assert.Equal(90, updated.DurationMinutes);
        Assert.Equal(200m, updated.Price);
    }
    
    [Fact]
    public async Task Put_NonExistingService_Returns404()
    {
        var request = new { name = "Cokolwiek", category = "Haircut", durationMinutes = 60, price = 100m };

        var response = await _client.PutAsJsonAsync($"/api/services/{Guid.NewGuid()}", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithInvalidDuration_Returns400AndKeepsOldValues()
    {
        var id = await _client.CreateServiceAsync(name: "Strzyżenie", durationMinutes: 60);
        var request = new { name = "Nowa nazwa", category = "Haircut", durationMinutes = 20, price = 100m };

        var response = await _client.PutAsJsonAsync($"/api/services/{id}", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var unchanged = await _client.GetFromJsonAsync<ServiceResponse>($"/api/services/{id}", TestJson.Options);
        Assert.Equal("Strzyżenie", unchanged!.Name);
        Assert.Equal(60, unchanged.DurationMinutes);
    }
}