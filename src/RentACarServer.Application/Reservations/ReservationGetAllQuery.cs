using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Customers;
using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Domain.RentalExtras;
using RentACarServer.Domain.Reservations;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using RentACarServer.Application.Behaviors;

namespace RentACarServer.Application.Reservations;

[Permission("reservation:view")]
public sealed record ReservationGetAllQuery : IRequest<IQueryable<ReservationDto>>;

internal sealed class ReservationGetAllQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IBranchRepository branchRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository,
    IProtectionPackageRepository protectionPackageRepository,
    IRentalExtraRepository extraRepository
) : IRequestHandler<ReservationGetAllQuery, IQueryable<ReservationDto>>
{
    public Task<IQueryable<ReservationDto>> Handle(ReservationGetAllQuery request, CancellationToken cancellationToken) => Task.FromResult(reservationRepository.GetAllWithAudit()
        .MapTo(
            customerRepository.GetAll(),
            branchRepository.GetAll(),
            vehicleRepository.GetAll(),
            categoryRepository.GetAll(),
            protectionPackageRepository.GetAll(),
            extraRepository.GetAll())
        .AsQueryable());
}