using System.Net;
using System.Net.Http.Json;
using SalonBooking.Api.Application.ServiceCatalog;
using SalonBooking.Api.Domain.ServiceCatalog;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests.Services;

[Collection(nameof(ApiCollection))]
public class GetServicesTests(SalonBookingApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    
    private async Task<List<ServiceResponse>> GetListAsync(string url)
    {
        var response = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<List<ServiceResponse>>(TestJson.Options))!;
    }
    
    [Fact]
    public async Task Get_WithoutFilter_ReturnsCreatedServices()
    {
        var haircutId = await _client.CreateServiceAsync(category: "Haircut");
        var coloringId = await _client.CreateServiceAsync(category: "Coloring");

        var list = await GetListAsync("/api/services");

        Assert.Contains(list, s => s.Id == haircutId);
        Assert.Contains(list, s => s.Id == coloringId);
    }
    
    [Fact]
    public async Task Get_WithCategoryFilter_ReturnsOnlyThatCategory()
    {
        var haircutId = await _client.CreateServiceAsync(category: "Haircut");
        var coloringId = await _client.CreateServiceAsync(category: "Coloring");

        var list = await GetListAsync("/api/services?category=Coloring");

        Assert.Contains(list, s => s.Id == coloringId);
        Assert.DoesNotContain(list, s => s.Id == haircutId);
        Assert.All(list, s => Assert.Equal(ServiceCategory.Coloring, s.Category));
    }
    
    [Fact]
    public async Task Get_ReturnsServicesSortedByName()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var secondId = await _client.CreateServiceAsync(name: $"B {suffix}");
        var firstId = await _client.CreateServiceAsync(name: $"A {suffix}");

        var list = await GetListAsync("/api/services");

        var firstIndex = list.FindIndex(s => s.Id == firstId);
        var secondIndex = list.FindIndex(s => s.Id == secondId);
        Assert.True(firstIndex < secondIndex, "Service 'A' should come before 'B'.");
    }
    
    [Fact]
    public async Task Get_WithUnknownCategory_Returns400()
    {
        var response = await _client.GetAsync("/api/services?category=Xyz");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}