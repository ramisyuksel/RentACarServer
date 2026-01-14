using RentACarServer.Domain.Roles;
using TS.MediatR;

namespace RentACarServer.Application.Roles;

public sealed record RoleGetAllQuery : IRequest<IQueryable<RoleDto>>;

internal sealed class RoleGetAllQueryHandler(IRoleRepository roleRepository) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleDto>>
{
    public Task<IQueryable<RoleDto>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(roleRepository.GetAllWithAudit().MapTo().AsQueryable());
}