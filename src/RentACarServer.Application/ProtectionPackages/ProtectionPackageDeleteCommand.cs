using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.ProtectionPackage;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.ProtectionPackages;

[Permission("protection_package:delete")]
public sealed record ProtectionPackageDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class ProtectionPackageDeleteCommandHandler(
    IProtectionPackageRepository protectionPackageRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ProtectionPackageDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageDeleteCommand request, CancellationToken cancellationToken)
    {
        var package = await protectionPackageRepository.FirstOrDefaultAsync(
            p => p.Id == request.Id,
            cancellationToken);

        if (package is null)
        {
            return Result<string>.Failure("Protection package not found");
        }

        package.Delete();
        protectionPackageRepository.Update(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package deleted successfully.";
    }
}