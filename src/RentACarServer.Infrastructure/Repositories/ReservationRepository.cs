using RentACarServer.Domain.Reservations;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class ReservationRepository(ApplicationDbContext context)
    : AuditableRepository<Reservation, ApplicationDbContext>(context), IReservationRepository;