using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileTagConfiguration : IEntityTypeConfiguration<ProfileTag>
{
    public void Configure(EntityTypeBuilder<ProfileTag> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("ProfileTags");
        builder.Property<byte>("Order");
        builder.HasOne(profileTag => profileTag.Tag).WithMany();
        builder.HasIndex("ProfileId", "TagId").IsUnique();
        builder.ComplexProperty(profileTag => profileTag.Offset, offsetBuilder =>
        {
	        offsetBuilder.Property(offset => offset.X).HasColumnName("HorizontalOffset");
	        offsetBuilder.Property(offset => offset.Y).HasColumnName("VerticalOffset");
        });
    }
}