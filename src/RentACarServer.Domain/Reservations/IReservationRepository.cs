using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.Reservations;

public interface IReservationRepository : IAuditableRepository<Reservation>;