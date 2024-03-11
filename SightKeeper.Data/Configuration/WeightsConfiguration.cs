using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasFlakeIdKey();
        builder.Navigation(weights => weights.Library).AutoInclude();
        builder.HasOne<WeightsData>("PTData").WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasOne<WeightsData>("ONNXData").WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasMany(weights => weights.ItemClasses).WithMany().UsingEntity<WeightsItemClass>(
            "WeightsItemClasses",
            left => left.HasOne(l => l.ItemClass).WithMany().HasForeignKey("ItemClassId").HasPrincipalKey("Id"),
            right => right.HasOne(r => r.Weights).WithMany().HasForeignKey("WeightsId").HasPrincipalKey("Id"),
            join => join.HasKey("WeightsId", "ItemClassId"));
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        builder.Navigation(weights => weights.ItemClasses).AutoInclude();
        builder.Property(weights => weights.WeightsMetrics.Epoch).HasColumnName(nameof(WeightsMetrics.Epoch));
        builder.Property(weights => weights.WeightsMetrics.LossMetrics.BoundingLoss).HasColumnName(nameof(LossMetrics.BoundingLoss));
        builder.Property(weights => weights.WeightsMetrics.LossMetrics.ClassificationLoss).HasColumnName(nameof(LossMetrics.ClassificationLoss));
        builder.Property(weights => weights.WeightsMetrics.LossMetrics.DeformationLoss).HasColumnName(nameof(LossMetrics.DeformationLoss));
    }

    public sealed class WeightsItemClass
    {
        public Weights Weights { get; private set; } = null!;
        public ItemClass ItemClass { get; private set; } = null!;
    }
}