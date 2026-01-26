namespace RentACarServer.Domain.Reservations.ValueObjects;

public sealed record PaymentInformation(string CardNumber, string Owner);