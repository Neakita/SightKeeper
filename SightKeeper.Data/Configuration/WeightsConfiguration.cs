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
        builder.HasMany(weights => weights.Tags).WithMany().UsingEntity<WeightsTag>(
            "WeightsTags",
            left => left.HasOne(l => l.Tag).WithMany().HasForeignKey("TagId").HasPrincipalKey("Id"),
            right => right.HasOne(r => r.Weights).WithMany().HasForeignKey("WeightsId").HasPrincipalKey("Id"),
            join => join.HasKey("WeightsId", "TagId"));
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        builder.Navigation(weights => weights.Tags).AutoInclude();
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

    public sealed class WeightsTag
    {
        public Weights Weights { get; private set; } = null!;
        public Tag Tag { get; private set; } = null!;
    }
}