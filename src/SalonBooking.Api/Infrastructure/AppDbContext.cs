using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Application.Abstractions;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}