using RentACarServer.Domain.RentalExtras;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class RentalExtraRepository(ApplicationDbContext context)
    : AuditableRepository<RentalExtra, ApplicationDbContext>(context), IRentalExtraRepository;