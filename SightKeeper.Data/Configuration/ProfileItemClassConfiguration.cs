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
        builder.HasOne(profileItemClass => profileItemClass.ItemClass).WithMany();
        builder.HasIndex("ProfileId", "ItemClassId").IsUnique();
    }
}