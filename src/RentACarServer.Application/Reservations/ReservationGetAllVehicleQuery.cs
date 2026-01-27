using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Vehicles;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Reservations;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Reservations;

public sealed record ReservationGetAllVehicleQuery(
    Guid BranchId,
    DateOnly PickUpDate,
    TimeOnly PickUpTime,
    DateOnly DeliveryDate,
    TimeOnly DeliverTime
) : IRequest<Result<List<VehicleDto>>>;

internal sealed class ReservationGetAllVehicleQueryHandler(
    IReservationRepository reservationRepository,
    IVehicleRepository vehicleRepository,
    IBranchRepository branchRepository,
    ICategoryRepository categoryRepository
) : IRequestHandler<ReservationGetAllVehicleQuery, Result<List<VehicleDto>>>
{
    public async Task<Result<List<VehicleDto>>> Handle(ReservationGetAllVehicleQuery request, CancellationToken cancellationToken)
    {
        var pickupDatetime = new DateTime(request.PickUpDate, request.PickUpTime);
        var deliveryDatetime = new DateTime(request.DeliveryDate, request.DeliverTime);
        var unavailableVehicleIdsQueryable = reservationRepository
            .Where(p =>
                p.PickUpLocationId == request.BranchId
                && p.PickUpDatetime.Value >= pickupDatetime
                && p.DeliveryDatetime.Value.AddHours(1) <= deliveryDatetime)
            .AsQueryable();

        var unavailabeVehicleIds = await unavailableVehicleIdsQueryable
            .Select(s => s.VehicleId.Value)
            .ToListAsync(cancellationToken);

        var vehicles = await vehicleRepository
            .GetAllWithAudit()
            .Where(p =>
                !unavailabeVehicleIds.Contains(p.Entity.Id)
                && p.Entity.BranchId.Value == request.BranchId)
            .MapTo(
                branchRepository.GetAll(),
                categoryRepository.GetAll())
            .ToListAsync(cancellationToken);

        return vehicles;
    }
}