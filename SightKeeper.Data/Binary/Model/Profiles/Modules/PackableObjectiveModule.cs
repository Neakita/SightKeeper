using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Model.Profiles.Modules;

/// <summary>
/// MemoryPackable version of <see cref="ObjectiveModule"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(1, typeof(PackableDetectorModule))]
[MemoryPackUnion(2, typeof(PackablePoser2DModule))]
[MemoryPackUnion(3, typeof(PackablePoser3DModule))]
internal abstract partial class PackableObjectiveModule : PackableModule
{
	public PackableActiveScalingOptions? ActiveScalingOptions { get; }
	public PackableActiveWalkingOptions? ActiveWalkingOptions { get; }
	public PackableBehavior Behavior { get; }

	public PackableObjectiveModule(
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		PackableActiveScalingOptions? activeScalingOptions,
		PackableActiveWalkingOptions? activeWalkingOptions,
		PackableBehavior behavior)
		: base(weightsId, passiveScalingOptions, passiveWalkingOptions)
	{
		ActiveScalingOptions = activeScalingOptions;
		ActiveWalkingOptions = activeWalkingOptions;
		Behavior = behavior;
	}
}