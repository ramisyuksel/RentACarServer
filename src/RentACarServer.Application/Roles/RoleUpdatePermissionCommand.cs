using GenericRepository;
using RentACarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

public sealed record RoleUpdatePermissionCommand(Guid RoleId, List<string> Permissions) : IRequest<Result<string>>;

internal sealed class RoleUpdatePermissionCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdatePermissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
        {
            return Result<string>.Failure($"Role with ID {request.RoleId} not found.");
        }

        List<Permission> permissions = request.Permissions
            .Select(p => new Permission(p))
            .ToList();

        role.SetPermissions(permissions);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Permissions updated successfully.";
    }
}