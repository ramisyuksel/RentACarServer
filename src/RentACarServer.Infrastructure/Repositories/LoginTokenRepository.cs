using GenericRepository;
using RentACarServer.Domain.LoginTokens;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class LoginTokenRepository(ApplicationDbContext context)
    : Repository<LoginToken, ApplicationDbContext>(context), ILoginTokenRepository;