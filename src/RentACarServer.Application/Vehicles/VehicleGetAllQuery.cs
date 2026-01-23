using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;

namespace RentACarServer.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetAllQuery : IRequest<IQueryable<VehicleDto>>;

internal sealed class VehicleGetAllQueryHandler(
    IVehicleRepository vehicleRepository) : IRequestHandler<VehicleGetAllQuery, IQueryable<VehicleDto>>
{
    public Task<IQueryable<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(vehicleRepository.GetAllWithAudit().MapTo().AsQueryable());
}