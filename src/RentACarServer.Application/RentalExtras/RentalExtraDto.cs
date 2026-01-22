using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.RentalExtras;

namespace RentACarServer.Application.RentalExtras;

public sealed class RentalExtraDto : EntityDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
}

public static class ExtraExtensions
{
    public static IQueryable<RentalExtraDto> MapTo(this IQueryable<EntityWithAuditDto<RentalExtra>> entities)
    {
        return entities.Select(s => new RentalExtraDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            Price = s.Entity.Price.Value,
            Description = s.Entity.Description.Value,
            IsActive = s.Entity.IsActive,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,
            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
        });
    }
}