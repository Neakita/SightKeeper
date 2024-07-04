using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorWeightsLibraryConfiguration : IEntityTypeConfiguration<DetectorWeightsLibrary>
{
	public void Configure(EntityTypeBuilder<DetectorWeightsLibrary> builder)
	{
		builder.HasMany<DetectorWeights>("_weights").WithOne(weights => weights.Library);
		builder.ToTable("DetectorWeightsLibraries");
	}
}