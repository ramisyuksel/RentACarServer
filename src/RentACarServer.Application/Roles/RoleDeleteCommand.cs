using GenericRepository;
using RentACarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

public sealed record RoleDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (role == null)
        {
            return Result<string>.Failure("Role not found");
        }

        role.Delete();
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Role deleted successfully";
    }
}