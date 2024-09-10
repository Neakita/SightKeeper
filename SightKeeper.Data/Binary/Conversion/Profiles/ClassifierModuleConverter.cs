using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ClassifierModuleConverter : ModuleConverter
{
	protected override PackableClassifierModule CreateModule(
		Module module,
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		ConversionSession session)
	{
		var typedModule = (ClassifierModule)module;
		var behavior = ConvertTriggerBehavior(typedModule.Behavior);
		return new PackableClassifierModule(weightsId, passiveScalingOptions, passiveWalkingOptions, behavior);
	}
}