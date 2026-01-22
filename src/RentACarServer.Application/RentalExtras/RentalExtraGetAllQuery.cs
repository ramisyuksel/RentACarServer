using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.RentalExtras;
using TS.MediatR;

namespace RentACarServer.Application.RentalExtras;

[Permission("rental_extra:view")]
public sealed record RentalExtraGetAllQuery : IRequest<IQueryable<RentalExtraDto>>;
internal sealed class RentalExtraGetAllQueryHandler(
    IRentalExtraRepository repository) : IRequestHandler<RentalExtraGetAllQuery, IQueryable<RentalExtraDto>>
{
    public Task<IQueryable<RentalExtraDto>> Handle(RentalExtraGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
}