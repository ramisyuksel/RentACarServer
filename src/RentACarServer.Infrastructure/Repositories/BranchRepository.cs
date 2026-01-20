using RentACarServer.Domain.Branches;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class BranchRepository : AuditableRepository<Branch, ApplicationDbContext>, IBranchRepository
{
    public BranchRepository(ApplicationDbContext context) : base(context)
    {
    }
}