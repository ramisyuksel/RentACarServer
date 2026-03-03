using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Customers;
using RentACarServer.Domain.Reservations;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Reservations.Forms;

public sealed record FormGetQuery(
    Guid ReservationId,
    string Type) : IRequest<Result<FormDto>>;

internal sealed class FormGetQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository
) : IRequestHandler<FormGetQuery, Result<FormDto>>
{
    public async Task<Result<FormDto>> Handle(FormGetQuery request, CancellationToken cancellationToken)
    {
        var res = await reservationRepository.GetAll().MapTo(
                request.Type,
                customerRepository.GetAll(),
                vehicleRepository.GetAll(),
                categoryRepository.GetAll())
            .Where(i => i.ReservationId == request.ReservationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
        {
            return Result<FormDto>.Failure("Rezervasyon bulunamadı");
        }

        return res;
    }
}