using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Domain.ProtectionPackage.ValueObjects;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.ProtectionPackages;

[Permission("protection_package:create")]
public sealed record ProtectionPackageCreateCommand(
    string Name,
    decimal Price,
    bool IsRecommended,
    bool IsActive,
    List<string>? Coverages
) : IRequest<Result<string>>;

public sealed class ProtectionPackageCreateCommandValidator : AbstractValidator<ProtectionPackageCreateCommand>
{
    public ProtectionPackageCreateCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Protection package name cannot be empty");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");
    }
}

internal sealed class ProtectionPackageCreateCommandHandler(
    IProtectionPackageRepository protectionPackageRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ProtectionPackageCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await protectionPackageRepository.AnyAsync(
            p => p.Name.Value == request.Name,
            cancellationToken);

        if (nameExists)
        {
            return Result<string>.Failure($"Protection package with name '{request.Name}' already exists.");
        }

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);

        var coverages = (request.Coverages ?? new List<string>())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct()
            .Select(s => new ProtectionCoverage(s));

        ProtectionPackage protectionPackage = new(name, price, isRecommended, coverages, request.IsActive);

        protectionPackageRepository.Add(protectionPackage);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package is created successfully.";
    }
}