using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class DetectorModuleConverter : ObjectiveModuleConverter
{
	public DetectorModuleConverter(ConversionSession session) : base(session)
	{
	}

	protected override PackableDetectorModule CreateModule(
		ObjectiveModule module,
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		PackableActiveScalingOptions? activeScalingOptions,
		PackableActiveWalkingOptions? activeWalkingOptions,
		PackableBehavior behavior)
	{
		return new PackableDetectorModule(
			weightsId, 
			passiveScalingOptions,
			passiveWalkingOptions,
			activeScalingOptions,
			activeWalkingOptions,
			behavior);
	}
}