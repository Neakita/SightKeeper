using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
internal sealed partial class SerializableTrackingWalkingOptions : SerializableActiveWalkingOptions
{
	public Vector2<float> MaximumOffset { get; }

	[MemoryPackConstructor]
	public SerializableTrackingWalkingOptions(Vector2<float> maximumOffset)
	{
		MaximumOffset = maximumOffset;
	}

	public SerializableTrackingWalkingOptions(TrackingWalkingOptions options)
	{
		MaximumOffset = options.MaximumOffset;
	}
}