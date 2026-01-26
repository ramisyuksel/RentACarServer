using RentACarServer.Domain.Users;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) : AuditableRepository<User, ApplicationDbContext>(context), IUserRepository;