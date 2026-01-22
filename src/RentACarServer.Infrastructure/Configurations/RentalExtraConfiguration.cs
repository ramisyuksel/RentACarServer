using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.RentalExtras;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class RentalExtraConfiguration : IEntityTypeConfiguration<RentalExtra>
{
    public void Configure(EntityTypeBuilder<RentalExtra> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.Name);
        builder.OwnsOne(x => x.Price);
        builder.OwnsOne(x => x.Description);
    }
}