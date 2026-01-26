using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Services;
using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Shared;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Users;
[Permission("user:create")]
public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");

    }
}

public sealed class UserCreateCommandHandler(
    IUserRepository userRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var emailExist = await userRepository.AnyAsync(u => u.Email.Value == request.Email, cancellationToken);
        if (emailExist)
            return Result<string>.Failure("Email is already in use.");

        var userNameExist = await userRepository.AnyAsync(u => u.UserName.Value == request.UserName, cancellationToken);
        if (userNameExist)
            return Result<string>.Failure("Username is already in use.");

        var branchId = claimContext.GetBranchId();
        if (request.BranchId is not null)
            branchId = request.BranchId.Value;

        FirstName firstName = new(request.FirstName);
        LastName lastName = new(request.LastName);
        Email email = new(request.Email);
        UserName userName = new(request.UserName);
        Password password = new("123");
        IdentityId branchIdRecord = new(branchId);
        IdentityId roleId = new(request.RoleId);

        User newUser = new(
            firstName,
            lastName,
            email,
            userName,
            password,
            branchIdRecord,
            roleId,
            request.IsActive);
        await userRepository.AddAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarı ile oluşturuldu : " + newUser.Id.Value;
    }
}