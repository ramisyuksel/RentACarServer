using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Vehicles;

[Permission("vehicle:delete")]
public sealed record VehicleDeleteCommand(
    Guid Id
) : IRequest<Result<string>>;

public sealed class VehicleDeleteCommandValidator : AbstractValidator<VehicleDeleteCommand>
{
    public VehicleDeleteCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

internal sealed class VehicleDeleteCommandHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<VehicleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleDeleteCommand request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await vehicleRepository.GetByExpressionAsync(v => v.Id.Value == request.Id, cancellationToken);
        if (vehicle is null)
            return Result<string>.Failure("Vehicle not found.");

        vehicle.Delete();

        vehicleRepository.Update(vehicle);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Vehicle deleted successfully";
    }
}