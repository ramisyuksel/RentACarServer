using RentACarServer.Application.Services;
using RentACarServer.Domain.Customers;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Dashboards;

public sealed record DashboardTotalCustomerCountQuery : IRequest<Result<int>>;

internal sealed class DashboardTotalCustomerCountQueryHandler(
    ICustomerRepository customerRepository) : IRequestHandler<DashboardTotalCustomerCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(DashboardTotalCustomerCountQuery request, CancellationToken cancellationToken)
    {
        var res = await customerRepository
            .CountAsync(cancellationToken);
        
        return res;
    }
}