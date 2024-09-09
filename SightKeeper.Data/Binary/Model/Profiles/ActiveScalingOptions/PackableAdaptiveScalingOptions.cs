using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;

/// <summary>
/// MemoryPackable version of <see cref="AdaptiveScalingOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableAdaptiveScalingOptions : PackableActiveScalingOptions
{
	public float Margin { get; }
	public float MaximumScaling { get; }

	public PackableAdaptiveScalingOptions(float margin, float maximumScaling)
	{
		Margin = margin;
		MaximumScaling = maximumScaling;
	}
}