using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.ProtectionPackage;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class ProtectionPackageConfiguration : IEntityTypeConfiguration<ProtectionPackage>
{
    public void Configure(EntityTypeBuilder<ProtectionPackage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(p => p.Name);
        builder.OwnsOne(p => p.Price);
        builder.OwnsOne(p => p.IsRecommended);
        builder.OwnsMany(p => p.Coverages);
        builder.OwnsOne(x => x.OrderNumber);
    }
}