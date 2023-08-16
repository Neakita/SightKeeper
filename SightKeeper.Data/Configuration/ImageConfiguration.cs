using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasShadowKey();
        builder.ToTable("Images");
        builder.OwnsOne(image => image.Resolution);
    }
}