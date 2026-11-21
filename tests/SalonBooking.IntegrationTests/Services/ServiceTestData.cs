using System.Net.Http.Json;
using SalonBooking.Api.Controllers;
using SalonBooking.IntegrationTests.Infrastructure;

namespace SalonBooking.IntegrationTests.Services;

public static class ServiceTestData
{
    public static async Task<Guid> CreateServiceAsync(
        this HttpClient client,
        string? name = null,
        string category = "Haircut",
        int durationMinutes = 60,
        decimal price = 120m)
    {
        var request = new
        {
            name = name ?? $"Usługa {Guid.NewGuid():N}",   
            category,
            durationMinutes,
            price
        };

        var response = await client.PostAsJsonAsync("/api/services", request);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<CreateServiceResponse>(TestJson.Options);
        return body!.Id;
    }
}