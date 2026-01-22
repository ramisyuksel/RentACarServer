using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.RentalExtras;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.RentalExtras;

[Permission("rental_extra:edit")]
public sealed record RentalExtraUpdateCommand(
    Guid Id,
    string Name,
    decimal Price,
    string Description,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class RentalExtraUpdateCommandValidator : AbstractValidator<RentalExtraUpdateCommand>
{
    public RentalExtraUpdateCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Rental extra name cannot be empty");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");

        RuleFor(p => p.Description).MaximumLength(500);
    }
}

internal sealed class RentalExtraUpdateCommandHandler(
    IRentalExtraRepository rentalExtraRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RentalExtraUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RentalExtraUpdateCommand request, CancellationToken cancellationToken)
    {
        var extra = await rentalExtraRepository.FirstOrDefaultAsync(
            p => p.Id == request.Id,
            cancellationToken);

        if (extra is null)
        {
            return Result<string>.Failure("Rental extra not found");
        }

        // Name uniqueness check only if changed
        if (!string.Equals(extra.Name.Value, request.Name, StringComparison.Ordinal))
        {
            var existsWithNewName = await rentalExtraRepository.AnyAsync(
                p => p.Name.Value == request.Name && p.Id != request.Id,
                cancellationToken);

            if (existsWithNewName)
            {
                return Result<string>.Failure($"Rental extra with name '{request.Name}' already exists.");
            }
        }

        Name name = new(request.Name);
        Price price = new(request.Price);
        Description description = new(request.Description);

        extra.SetName(name);
        extra.SetPrice(price);
        extra.SetDescription(description);
        extra.SetStatus(request.IsActive);

        rentalExtraRepository.Update(extra);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rental extra updated successfully.";
    }
}