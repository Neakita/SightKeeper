using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
internal sealed partial class TrackingWalkingOptions : ActiveWalkingOptions
{
	public Vector2<float> MaximumOffset { get; }

	[MemoryPackConstructor]
	public TrackingWalkingOptions(Vector2<float> maximumOffset)
	{
		MaximumOffset = maximumOffset;
	}

	public TrackingWalkingOptions(Domain.Model.Profiles.Modules.Walking.TrackingWalkingOptions options)
	{
		MaximumOffset = options.MaximumOffset;
	}
}