using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentACarServer.Application.Services;
using RentACarServer.Domain.LoginTokens;
using RentACarServer.Domain.LoginTokens.ValueObjects;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Users;
using RentACarServer.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RentACarServer.Infrastructure.Services;

internal sealed class JwtProvider(
    ILoginTokenRepository loginTokenRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IOptions<JwtOptions> options) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.FirstOrDefaultAsync(i => i.Id == user.RoleId, cancellationToken);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("fullName", user.FullName.Value),
            new Claim("email",user.Email.Value),
            new Claim("role", role.Name.Value),
            new Claim("permissions", JsonSerializer.Serialize(role.Permissions))
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);
        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(securityToken);

        Token newToken = new(token);
        ExpiresDate expiresDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expiresDate);
        await loginTokenRepository.AddAsync(loginToken, cancellationToken);


        // this codes deactivate all previous tokens, if you want to allow multiple tokens (devices, browsers, etc.), remove this part
        var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);

        foreach (var item in loginTokens)
        {
            item.SetIsActive(new(false));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}