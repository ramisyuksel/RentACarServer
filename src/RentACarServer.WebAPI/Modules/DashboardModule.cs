using RentACarServer.Application.Dashboards;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebAPI.Modules;

public static class DashboardModule
{
    public static void MapDashboard(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/dashboard")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Dashboard");

        app.MapGet("active-reservation-count",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new DashboardActiveReservationCountQuery(), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<int>>();

        app.MapGet("total-vehicle-count",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new DashboardTotalVehicleCountQuery(), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<int>>();

        app.MapGet("daily-income",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new DashboardDailyIncomeQuery(), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<decimal>>();

        app.MapGet("total-customer-count",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new DashboardTotalCustomerCountQuery(), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<int>>();
    }
}