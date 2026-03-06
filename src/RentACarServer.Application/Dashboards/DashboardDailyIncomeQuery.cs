using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Services;
using RentACarServer.Domain.Reservations;
using RentACarServer.Domain.Reservations.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Dashboards;

public sealed record DashboardDailyIncomeQuery : IRequest<Result<decimal>>;

internal sealed class DashboardDailyIncomeQueryHandler(
    IReservationRepository reservationRepository, 
    IClaimContext claimContext) : IRequestHandler<DashboardDailyIncomeQuery, Result<decimal>>
{
    public async Task<Result<decimal>> Handle(DashboardDailyIncomeQuery request, CancellationToken cancellationToken)
    {
        var branchId = claimContext.GetBranchId();

        var res = await reservationRepository
            .GetAll()
            .Where(p => p.PickUpLocationId == branchId)
            .Where(p => p.PickUpDate.Value == DateOnly.FromDateTime(DateTime.Now))
            .SumAsync(p => p.Total.Value, cancellationToken);

        return res;
    }
}