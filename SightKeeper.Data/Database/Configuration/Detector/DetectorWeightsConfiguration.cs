using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorWeightsConfiguration : IEntityTypeConfiguration<DetectorWeights>
{
	public void Configure(EntityTypeBuilder<DetectorWeights> builder)
	{
		builder.ToTable("DetectorWeights");
	}
}