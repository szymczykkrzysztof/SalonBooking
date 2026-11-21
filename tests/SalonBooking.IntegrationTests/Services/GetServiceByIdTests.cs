using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests.Services;

[Collection(nameof(ApiCollection))]
public class GetServiceByIdTests(SalonBookingApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    [Fact]
    public async Task Get_ExistingService_Returns200WithData()
    {
        // Arrange
        var id = await _client.CreateServiceAsync(
            name: "Koloryzacja", category: "Coloring", durationMinutes: 120, price: 250m);

        // Act
        var response = await _client.GetAsync($"/api/services/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var service = await response.Content.ReadFromJsonAsync<ServiceResponse>(TestJson.Options);
        Assert.Equal(id, service!.Id);
        Assert.Equal("Koloryzacja", service.Name);
        Assert.Equal(ServiceCategory.Coloring, service.Category);
        Assert.Equal(120, service.DurationMinutes);
        Assert.Equal(250m, service.Price);
    }
    
    [Fact]
    public async Task Get_NonExistingService_Returns404WithProblemDetails()
    {
        var response = await _client.GetAsync($"/api/services/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestJson.Options);
        Assert.Equal("Service.NotFound", problem!.Title);
    }
    
    [Fact]
    public async Task Get_IdNotAGuid_Returns404()
    {
        var response = await _client.GetAsync("/api/services/abc");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}