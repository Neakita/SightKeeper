using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Model.Profiles.Modules;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierModule"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierModule : PackableModule
{
	public PackableTriggerBehavior Behavior { get; }

	public PackableClassifierModule(
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		PackableTriggerBehavior behavior)
		: base(weightsId, passiveScalingOptions, passiveWalkingOptions)
	{
		Behavior = behavior;
	}
}