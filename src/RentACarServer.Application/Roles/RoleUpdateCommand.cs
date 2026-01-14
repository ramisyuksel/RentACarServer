using FluentValidation;
using GenericRepository;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

public sealed record RoleUpdateCommand(Guid Id, string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Role name cannot be empty");
    }
}

internal sealed class RoleUpdateCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role == null) {
            return Result<string>.Failure("Role not found");
        }

        Name name = new(request.Name);
        role.SetName(name);
        role.SetStatus(request.IsActive);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Role updated successfully";
    }
}