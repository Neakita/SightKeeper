using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasFlakeIdKey();
        builder.HasMany<DbWeightsData>().WithOne(weightsData => weightsData.Weights);
        builder.HasMany(weights => weights.ItemClasses).WithMany().UsingEntity<WeightsItemClass>(
            "WeightsItemClasses",
            left => left.HasOne(l => l.ItemClass).WithMany().HasForeignKey("ItemClassId").HasPrincipalKey("Id"),
            right => right.HasOne(r => r.Weights).WithMany().HasForeignKey("WeightsId").HasPrincipalKey("Id"),
            join => join.HasKey("WeightsId", "ItemClassId"));
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        builder.Navigation(weights => weights.ItemClasses).AutoInclude();
        builder.ComplexProperty(weights => weights.WeightsMetrics, weightsMetricsBuilder =>
        {
	        weightsMetricsBuilder.Property(weightsMetrics => weightsMetrics.Epoch).HasColumnName(nameof(WeightsMetrics.Epoch));
	        weightsMetricsBuilder.ComplexProperty(weightsMetrics => weightsMetrics.LossMetrics, lossMetricsBuilder =>
	        {
		        lossMetricsBuilder.Property(lossMetrics => lossMetrics.BoundingLoss).HasColumnName(nameof(LossMetrics.BoundingLoss));
		        lossMetricsBuilder.Property(lossMetrics => lossMetrics.ClassificationLoss).HasColumnName(nameof(LossMetrics.ClassificationLoss));
		        lossMetricsBuilder.Property(lossMetrics => lossMetrics.DeformationLoss).HasColumnName(nameof(LossMetrics.DeformationLoss));
	        });
        });
    }

    public sealed class WeightsItemClass
    {
        public Weights Weights { get; private set; } = null!;
        public ItemClass ItemClass { get; private set; } = null!;
    }
}