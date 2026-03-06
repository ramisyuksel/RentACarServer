namespace RentACarServer.Domain.Reservations.Forms.ValueObjects;

public sealed record Damage(
    string Level,
    string Description);