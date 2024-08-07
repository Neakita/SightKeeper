using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Services;
using PoserModule = SightKeeper.Data.Binary.Profiles.Modules.PoserModule;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class PoserModulesConverter
{
	public PoserModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public PoserModule Convert(Domain.Model.Profiles.Modules.PoserModule module, ConversionSession session)
	{
		return new PoserModule(BehavioursConverter.Convert(module.Behaviour, session))
		{
			WeightsId = _weightsDataAccess.GetId(module.Weights),
			PassiveScalingOptions = PassiveScalingOptionsConverter.Convert(module.PassiveScalingOptions),
			PassiveWalkingOptions = PassiveWalkingOptionsConverter.Convert(module.PassiveWalkingOptions),
			ActiveScalingOptions = ActiveScalingOptionsConverter.Convert(module.ActiveScalingOptions),
			ActiveWalkingOptions = ActiveWalkingOptionsConverter.Convert(module.ActiveWalkingOptions)
		};
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
}