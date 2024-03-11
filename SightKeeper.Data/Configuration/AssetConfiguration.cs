using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("Assets");
        builder.HasMany(asset => asset.Items).WithOne(item => item.Asset);
    }
}