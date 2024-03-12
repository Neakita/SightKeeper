using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("Images");
        builder.Property(image => image.Content);
    }
}