using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;
[Permission("role:create")]
public sealed record RoleCreateCommand(string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Role name cannot be empty");
    }
}

internal sealed class RoleCreateCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await roleRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (nameExists)
        {
            return Result<string>.Failure($"Role with name '{request.Name}' already exists.");
        }

        Name name = new(request.Name);
        Role role = new(name, request.IsActive);

        roleRepository.Add(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Role is created successfully.";
    }
}