using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class DetectorModulesConverter
{
	public DetectorModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public SerializableDetectorModule Convert(DetectorModule module, ConversionSession session)
	{
		return new SerializableDetectorModule(BehavioursConverter.Convert(module.Behaviour, session))
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