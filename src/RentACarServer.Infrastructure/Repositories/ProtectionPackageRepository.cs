using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

public class ProtectionPackageRepository : AuditableRepository<ProtectionPackage, ApplicationDbContext>, IProtectionPackageRepository
{
    public ProtectionPackageRepository(ApplicationDbContext context) : base(context)
    {
    }
}