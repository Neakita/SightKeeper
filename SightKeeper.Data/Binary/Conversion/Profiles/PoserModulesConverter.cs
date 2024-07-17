using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class PoserModulesConverter
{
	public PoserModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public SerializablePoserModule Convert(PoserModule module, ConversionSession session)
	{
		return new SerializablePoserModule(BehavioursConverter.Convert(module.Behaviour, session))
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