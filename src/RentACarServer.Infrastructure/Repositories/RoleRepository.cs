using RentACarServer.Domain.Roles;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class RoleRepository(ApplicationDbContext context)
    : AuditableRepository<Role, ApplicationDbContext>(context), IRoleRepository;