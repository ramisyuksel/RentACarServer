using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class ProtectionPackageRepository(ApplicationDbContext context)
    : AuditableRepository<ProtectionPackage, ApplicationDbContext>(context), IProtectionPackageRepository;