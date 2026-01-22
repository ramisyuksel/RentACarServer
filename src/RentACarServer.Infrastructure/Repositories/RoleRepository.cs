using RentACarServer.Domain.Roles;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext context)
    : AuditableRepository<Role, ApplicationDbContext>(context), IRoleRepository;