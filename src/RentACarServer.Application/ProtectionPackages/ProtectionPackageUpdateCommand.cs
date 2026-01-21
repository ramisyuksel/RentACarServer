using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Domain.ProtectionPackage.ValueObjects;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.ProtectionPackages;

[Permission("protection_package:edit")]
public sealed record ProtectionPackageUpdateCommand(
    Guid Id,
    string Name,
    decimal Price,
    bool IsRecommended,
    bool IsActive,
    List<string>? Coverages) : IRequest<Result<string>>;

public sealed class ProtectionPackageUpdateCommandValidator : AbstractValidator<ProtectionPackageUpdateCommand>
{
    public ProtectionPackageUpdateCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Protection package name cannot be empty");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");
    }
}

internal sealed class ProtectionPackageUpdateCommandHandler(
    IProtectionPackageRepository protectionPackageRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ProtectionPackageUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageUpdateCommand request, CancellationToken cancellationToken)
    {
        var package = await protectionPackageRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (package is null)
            return Result<string>.Failure("Protection package not found");


        if (!string.Equals(package.Name.Value, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existsWithNewName = await protectionPackageRepository.AnyAsync(
                p => p.Name.Value == request.Name && p.Id != request.Id,
                cancellationToken);

            if (existsWithNewName)
                return Result<string>.Failure($"Protection package with name '{request.Name}' already exists.");
        }

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);
        List<ProtectionCoverage> coverages = request.Coverages.Select(c => new ProtectionCoverage(c)).ToList();

        package.SetName(name);
        package.SetPrice(price);
        package.SetIsRecommended(isRecommended);
        package.SetCoverages(coverages);
        package.SetStatus(request.IsActive);

        protectionPackageRepository.Update(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package updated successfully.";
    }
}