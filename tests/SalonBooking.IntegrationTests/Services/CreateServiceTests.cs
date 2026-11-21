using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalonBooking.Api.Controllers;
using SalonBooking.Api.Infrastructure;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests.Services;

[Collection(nameof(ApiCollection))]
public class CreateServiceTests(SalonBookingApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_WithValidData_Returns201AndSavesService()
    {
        var request = new {name = "Koloryzacja", category = "Coloring", durationMinutes = 120, price = 250.00m};
        var response = await _client.PostAsJsonAsync("/api/services", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CreateServiceResponse>(TestJson.Options);
        Assert.EndsWith($"/api/services/{body!.Id}", response.Headers.Location!.ToString());
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var saved = await db.Services.SingleAsync(s => s.Id == body.Id);
        Assert.Equal("Koloryzacja", saved.Name);
        Assert.Equal(250.00m, saved.DefaultPrice.Amount);
    }
    
    [Theory]
    [InlineData("", 60, 120)]           
    [InlineData("Strzyżenie", 0, 120)] 
    [InlineData("Strzyżenie", 20, 120)] 
    [InlineData("Strzyżenie", 60, 0)]   
    public async Task Post_WithInvalidData_Returns400(string name, int minutes, int price)
    {
        var request = new { name, category = "Haircut", durationMinutes = minutes, price };

        var response = await _client.PostAsJsonAsync("/api/services", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestJson.Options);
        Assert.NotEmpty(problem!.Errors);
    }
    
    [Fact]
    public async Task Post_WithNameOf101Characters_Returns400()
    {
        var request = new { name = new string('a', 101), category = "Haircut", durationMinutes = 60, price = 120 };

        var response = await _client.PostAsJsonAsync("/api/services", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}