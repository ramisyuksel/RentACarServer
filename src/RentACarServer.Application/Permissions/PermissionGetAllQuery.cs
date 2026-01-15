using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Services;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Permissions;
[Permission("permission:view")]
public sealed record PermissionGetAllQuery : IRequest<Result<List<string>>>;

internal sealed class PermissionGetAllQueryHandler(PermissionService permissionService) : IRequestHandler<PermissionGetAllQuery, Result<List<string>>>
{
    public Task<Result<List<string>>> Handle(PermissionGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(Result<List<string>>.Succeed(permissionService.GetAll()));
}