using Zin.Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zin.Infra.Database.Configurations;

public class EnrollmentTokenConfiguration : IEntityTypeConfiguration<EnrollmentToken>
{
    public void Configure(EntityTypeBuilder<EnrollmentToken> builder)
    {
        builder.ToTable("EnrollmentToken");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Token).IsUnique();
        builder.Property(x => x.Token).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);
    }
}
