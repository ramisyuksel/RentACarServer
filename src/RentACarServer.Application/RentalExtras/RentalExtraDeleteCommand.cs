using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.RentalExtras;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.RentalExtras;

[Permission("rental_extra:delete")]
public sealed record RentalExtraDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class RentalExtraDeleteCommandHandler(
    IRentalExtraRepository rentalExtraRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RentalExtraDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RentalExtraDeleteCommand request, CancellationToken cancellationToken)
    {
        var extra = await rentalExtraRepository.FirstOrDefaultAsync(
            p => p.Id == request.Id,
            cancellationToken);

        if (extra is null)
        {
            return Result<string>.Failure("Rental extra not found");
        }

        extra.Delete();
        rentalExtraRepository.Update(extra);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rental extra deleted successfully.";
    }
}