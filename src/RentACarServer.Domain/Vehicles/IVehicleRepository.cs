using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.Vehicles
{
    public interface IVehicleRepository : IAuditableRepository<Vehicle>
    {
    }
}