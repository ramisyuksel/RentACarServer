using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Customers;
using RentACarServer.Domain.ProtectionPackage;
using RentACarServer.Domain.RentalExtras;
using RentACarServer.Domain.Reservations;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Reservations;

public sealed record ReservationGetQuery(
    Guid Id) : IRequest<Result<ReservationDto>>;

internal sealed class ReservationGetQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IBranchRepository branchRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository,
    IProtectionPackageRepository protectionPackageRepository,
    IRentalExtraRepository extraRepository
) : IRequestHandler<ReservationGetQuery, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(ReservationGetQuery request, CancellationToken cancellationToken)
    {
        var res = await reservationRepository.GetAllWithAudit().MapTo(
                customerRepository.GetAll(),
                branchRepository.GetAll(),
                vehicleRepository.GetAll(),
                categoryRepository.GetAll(),
                protectionPackageRepository.GetAll(),
                extraRepository.GetAll())
            .Where(i => i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
        {
            return Result<ReservationDto>.Failure("Rezervasyon bulunamadı");
        }

        return res;
    }
}