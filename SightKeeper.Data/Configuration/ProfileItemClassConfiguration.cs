using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileItemClassConfiguration : IEntityTypeConfiguration<ProfileItemClass>
{
    public void Configure(EntityTypeBuilder<ProfileItemClass> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("ProfileItemClasses");
        builder.Property<byte>("Order");
        builder.HasOne(profileItemClass => profileItemClass.ItemClass).WithMany();
        builder.HasIndex("ProfileId", "ItemClassId").IsUnique();
        builder.ComplexProperty(profileItemClass => profileItemClass.Offset, offsetBuilder =>
        {
	        offsetBuilder.Property(offset => offset.X).HasColumnName("HorizontalOffset");
	        offsetBuilder.Property(offset => offset.Y).HasColumnName("VerticalOffset");
        });
    }
}