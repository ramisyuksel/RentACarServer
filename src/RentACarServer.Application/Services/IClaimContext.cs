namespace RentACarServer.Application.Services;

public interface IClaimContext
{
    Guid GetUserId();
    Guid GetBranchId();
    string GetRoleName();
}