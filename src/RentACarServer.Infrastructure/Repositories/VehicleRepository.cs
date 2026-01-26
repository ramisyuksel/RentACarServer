using RentACarServer.Domain.Vehicles;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories
{
    public sealed class VehicleRepository(ApplicationDbContext context)
        : AuditableRepository<Vehicle, ApplicationDbContext>(context), IVehicleRepository;
}