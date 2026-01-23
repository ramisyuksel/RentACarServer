using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetQuery(Guid Id) : IRequest<Result<VehicleDto>>;

internal sealed class VehicleGetQueryHandler(
    IVehicleRepository vehicleRepository,
    IBranchRepository branchRepository,
    ICategoryRepository categoryRepository) : IRequestHandler<VehicleGetQuery, Result<VehicleDto>>
{
    public async Task<Result<VehicleDto>> Handle(VehicleGetQuery request, CancellationToken cancellationToken)
    {
        var res = await vehicleRepository
            .GetAllWithAudit()
            .MapTo(branchRepository.GetAll(), categoryRepository.GetAll())
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<VehicleDto>.Failure("Vehicle not found");

        return res;
    }
}