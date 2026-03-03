namespace RentACarServer.Domain.Reservations;

public sealed record ReservationHistory(
    string Title,
    string Description,
    DateTimeOffset CreatedAt);