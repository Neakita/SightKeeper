using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;

/// <summary>
/// MemoryPackable version of <see cref="TrackingWalkingOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableTrackingWalkingOptions : PackableActiveWalkingOptions
{
	public Vector2<float> MaximumOffset { get; }

	public PackableTrackingWalkingOptions(Vector2<float> maximumOffset)
	{
		MaximumOffset = maximumOffset;
	}
}