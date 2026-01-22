using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.RentalExtras;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.RentalExtras;
[Permission("rental_extra:view")]
public sealed record RentalExtraGetQuery(Guid Id) : IRequest<Result<RentalExtraDto>>;
internal sealed class RentalExtraGetQueryHandler(
    IRentalExtraRepository repository) : IRequestHandler<RentalExtraGetQuery, Result<RentalExtraDto>>
{
    public async Task<Result<RentalExtraDto>> Handle(RentalExtraGetQuery request, CancellationToken cancellationToken)
    {
        var res = await repository
            .GetAllWithAudit()
            .MapTo()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<RentalExtraDto>.Failure("Ekstra bulunamadý");

        return res;
    }
}