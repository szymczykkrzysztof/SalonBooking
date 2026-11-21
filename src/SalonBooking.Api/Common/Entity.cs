namespace SalonBooking.Api.Common;

public abstract class Entity
{
    public Guid Id { get; protected init; }
}