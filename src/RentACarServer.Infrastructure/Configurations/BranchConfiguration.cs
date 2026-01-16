using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.Branches;

namespace RentACarServer.Infrastructure.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne(i => i.Name);
        builder.OwnsOne(i => i.Address);
        builder.OwnsOne(i => i.Contact);
    }
}