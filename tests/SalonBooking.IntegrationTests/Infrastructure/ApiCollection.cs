namespace SalonBooking.IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(ApiCollection))]
public sealed class ApiCollection : ICollectionFixture<SalonBookingApiFactory> { }