using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Services;
using ClassifierModule = SightKeeper.Data.Binary.Profiles.Modules.ClassifierModule;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ClassifierModulesConverter
{
	public ClassifierModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ClassifierModule Convert(Domain.Model.Profiles.Modules.ClassifierModule module, ConversionSession session)
	{
		return new ClassifierModule(TriggerBehaviourConverter.Convert(module.Behaviour, session))
		{
			WeightsId = _weightsDataAccess.GetId(module.Weights),
			PassiveScalingOptions = PassiveScalingOptionsConverter.Convert(module.PassiveScalingOptions),
			PassiveWalkingOptions = PassiveWalkingOptionsConverter.Convert(module.PassiveWalkingOptions)
		};
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
}