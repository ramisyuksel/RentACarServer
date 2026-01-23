using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;

namespace RentACarServer.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetAllQuery : IRequest<IQueryable<VehicleDto>>;

internal sealed class VehicleGetAllQueryHandler(
    IVehicleRepository vehicleRepository,
    IBranchRepository branchRepository,
    ICategoryRepository categoryRepository) : IRequestHandler<VehicleGetAllQuery, IQueryable<VehicleDto>>
{
    public Task<IQueryable<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(vehicleRepository.GetAllWithAudit().MapTo(branchRepository.GetAll(), categoryRepository.GetAll()).AsQueryable());
}