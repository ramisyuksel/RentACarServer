using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RentACarServer.Application.RentalExtras;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebAPI.Modules;

public static class RentalExtraModule
{
    public static void MapRentalExtra(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/rental-extras")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("RentalExtras");

        app.MapPost(string.Empty,
                async (RentalExtraCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<string>>();

        app.MapPut(string.Empty,
                async (RentalExtraUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<string>>();

        app.MapDelete("{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new RentalExtraDeleteCommand(id), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<string>>();

        app.MapGet("{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new RentalExtraGetQuery(id), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<RentalExtraDto>>();
    }
}