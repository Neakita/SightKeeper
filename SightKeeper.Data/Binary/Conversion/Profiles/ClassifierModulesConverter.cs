using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ClassifierModulesConverter
{
	public ClassifierModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public SerializableClassifierModule Convert(ClassifierModule module, ConversionSession session)
	{
		return new SerializableClassifierModule(TriggerBehaviourConverter.Convert(module.Behaviour, session))
		{
			WeightsId = _weightsDataAccess.GetId(module.Weights),
			PassiveScalingOptions = PassiveScalingOptionsConverter.Convert(module.PassiveScalingOptions),
			PassiveWalkingOptions = PassiveWalkingOptionsConverter.Convert(module.PassiveWalkingOptions)
		};
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
}