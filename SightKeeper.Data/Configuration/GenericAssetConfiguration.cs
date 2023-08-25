using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class GenericAssetConfiguration<TAsset> : IEntityTypeConfiguration<TAsset> where TAsset : Asset
{
    public void Configure(EntityTypeBuilder<TAsset> builder)
    {
        builder.ToTable("Assets");
    }
}