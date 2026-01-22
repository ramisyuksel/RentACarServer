using RentACarServer.Domain.Branches;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class BranchRepository(ApplicationDbContext context)
    : AuditableRepository<Branch, ApplicationDbContext>(context), IBranchRepository;