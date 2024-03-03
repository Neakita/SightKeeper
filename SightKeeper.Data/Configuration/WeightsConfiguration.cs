using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasKey(weights => weights.Id);
        builder.HasFlakeId(weights => weights.Id);
        builder.Navigation(weights => weights.Library).AutoInclude();
        builder.HasOne(weights => weights.PTWeightsData).WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasOne(weights => weights.ONNXWeightsData).WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasMany(weights => weights.ItemClasses).WithMany().UsingEntity<WeightsItemClass>(
            "WeightsItemClasses",
            left => left.HasOne(l => l.ItemClass).WithMany().HasForeignKey("ItemClassId").HasPrincipalKey("Id"),
            right => right.HasOne(r => r.Weights).WithMany().HasForeignKey("WeightsId").HasPrincipalKey("Id"),
            join => join.HasKey("WeightsId", "ItemClassId"));
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        builder.Navigation(weights => weights.ItemClasses).AutoInclude();
        builder.Property(weights => weights.WeightsMetrics.Epoch).HasColumnName(nameof(WeightsMetrics.Epoch));
        builder.Property(weights => weights.WeightsMetrics.BoundingLoss).HasColumnName(nameof(WeightsMetrics.BoundingLoss));
        builder.Property(weights => weights.WeightsMetrics.ClassificationLoss).HasColumnName(nameof(WeightsMetrics.ClassificationLoss));
        builder.Property(weights => weights.WeightsMetrics.DeformationLoss).HasColumnName(nameof(WeightsMetrics.DeformationLoss));
    }

    public sealed class WeightsItemClass : ObservableObject
    {
        public Weights Weights { get; private set; } = null!;
        public ItemClass ItemClass { get; private set; } = null!;
    }
}