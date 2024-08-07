using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Services;
using DetectorModule = SightKeeper.Data.Binary.Profiles.Modules.DetectorModule;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class DetectorModulesConverter
{
	public DetectorModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public DetectorModule Convert(Domain.Model.Profiles.Modules.DetectorModule module, ConversionSession session)
	{
		return new DetectorModule(BehavioursConverter.Convert(module.Behaviour, session))
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