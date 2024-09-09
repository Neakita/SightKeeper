using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Model.Profiles.Modules;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DModule"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DModule : PackableObjectiveModule
{
	public PackablePoser2DModule(
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		PackableActiveScalingOptions? activeScalingOptions,
		PackableActiveWalkingOptions? activeWalkingOptions,
		PackableBehavior behavior)
		: base(weightsId, passiveScalingOptions, passiveWalkingOptions, activeScalingOptions, activeWalkingOptions, behavior)
	{
	}
}