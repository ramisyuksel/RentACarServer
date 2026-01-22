using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.RentalExtras;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.RentalExtras;

[Permission("rental_extra:create")]
public sealed record RentalExtraCreateCommand(
    string Name,
    decimal Price,
    string Description,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class RentalExtraCreateCommandValidator : AbstractValidator<RentalExtraCreateCommand>
{
    public RentalExtraCreateCommandValidator()
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

internal sealed class RentalExtraCreateCommandHandler(
    IRentalExtraRepository rentalExtraRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RentalExtraCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RentalExtraCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await rentalExtraRepository.AnyAsync(
            p => p.Name.Value == request.Name,
            cancellationToken);

        if (nameExists)
        {
            return Result<string>.Failure($"Rental extra with name '{request.Name}' already exists.");
        }

        Name name = new(request.Name);
        Price price = new(request.Price);
        Description description = new(request.Description);

        var extra = new RentalExtra(name, price, description, request.IsActive);

        rentalExtraRepository.Add(extra);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rental extra is created successfully.";
    }
}