using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentACarServer.Application.Branches;
using RentACarServer.Application.Categories;
using RentACarServer.Application.ProtectionPackages;
using RentACarServer.Application.RentalExtras;
using RentACarServer.Application.Roles;
using RentACarServer.Application.Users;
using RentACarServer.Application.Vehicles;
using TS.MediatR;

namespace RentACarServer.WebAPI.Controllers
{
    [Route("odata")]
    [ApiController]
    [EnableQuery]
    public class MainODataController : ODataController
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EnableLowerCamelCase();
            builder.EntitySet<BranchDto>("branches");
            builder.EntitySet<RoleDto>("roles");
            builder.EntitySet<UserDto>("users");
            builder.EntitySet<CategoryDto>("categories");
            builder.EntitySet<ProtectionPackageDto>("protection-packages");
            builder.EntitySet<RentalExtraDto>("rental-extras");
            builder.EntitySet<VehicleDto>("vehicles");
            return builder.GetEdmModel();
        }

        [HttpGet("branches")]
        public IQueryable<BranchDto> Branches(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new BranchGetAllQuery(), cancellationToken).Result;

        [HttpGet("roles")]
        public IQueryable<RoleDto> Roles(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new RoleGetAllQuery(), cancellationToken).Result;

        [HttpGet("users")]
        public IQueryable<UserDto> Users(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new UserGetAllQuery(), cancellationToken).Result;

        [HttpGet("categories")]
        public IQueryable<CategoryDto> Categories(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new CategoryGetAllQuery(), cancellationToken).Result;

        [HttpGet("protection-packages")]
        public IQueryable<ProtectionPackageDto> ProtectionPackages(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new ProtectionPackageGetAllQuery(), cancellationToken).Result;

        [HttpGet("rental-extras")]
        public IQueryable<RentalExtraDto> RentalExtras(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new RentalExtraGetAllQuery(), cancellationToken).Result;

        [HttpGet("vehicles")]
        public IQueryable<VehicleDto> Vehicles(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new VehicleGetAllQuery(), cancellationToken).Result;
    }
}
