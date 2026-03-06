using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Services;
using RentACarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Dashboards;

public sealed record DashboardTotalVehicleCountQuery : IRequest<Result<int>>;

internal sealed class DashboardTotalVehicleCountQueryHandler(
    IVehicleRepository vehicleRepository,
    IClaimContext claimContext) : IRequestHandler<DashboardTotalVehicleCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(DashboardTotalVehicleCountQuery request, CancellationToken cancellationToken)
    {
        var branchId = claimContext.GetBranchId();

        var res = await vehicleRepository
            .GetAll()
            .Where(x => x.BranchId.Value == branchId)
            .CountAsync(cancellationToken);
        
        return res;
    }
}