using RentACarServer.Domain.RentalExtras;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

public class RentalExtraRepository(ApplicationDbContext context)
    : AuditableRepository<RentalExtra, ApplicationDbContext>(context), IRentalExtraRepository;