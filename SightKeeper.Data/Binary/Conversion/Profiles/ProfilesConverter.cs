using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Profiles;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ProfilesConverter
{
	public ProfilesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_modulesConverter = new ModulesConverter(weightsDataAccess);
	}
	
	public ImmutableArray<SerializableProfile> Convert(IEnumerable<Profile> profiles, ConversionSession session)
	{
		return profiles.Select(profile => Convert(profile, session)).ToImmutableArray();
	}

	public HashSet<Profile> ConvertBack(ImmutableArray<SerializableProfile> profiles, ReverseConversionSession session)
	{
		return profiles.Select(profile => ConvertBack(profile, session)).ToHashSet();
	}

	private readonly ModulesConverter _modulesConverter;

	private SerializableProfile Convert(Profile profile, ConversionSession session)
	{
		var modules = _modulesConverter.Convert(profile.Modules, session);
		return new SerializableProfile(profile.Name, modules);
	}

	private Profile ConvertBack(SerializableProfile profile, ReverseConversionSession session)
	{
		Profile converted = new(profile.Name);
		foreach (var module in profile.Modules)
			AddModule(converted, module, session);
		return converted;
	}

	private static void AddModule(Profile profile, SerializableModule module, ReverseConversionSession session)
	{
		switch (module)
		{
			case SerializableClassifierModule classifierModule:
				AddModule(profile, classifierModule, session);
				break;
			case SerializableDetectorModule detectorModule:
				AddModule(profile, detectorModule, session);
				break;
			case SerializablePoserModule poserModule:
				AddModule(profile, poserModule, session);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(module));
		}
	}

	private static void AddModule(Profile profile, SerializableClassifierModule module, ReverseConversionSession session)
	{
		var weights = (ClassifierWeights)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		var actions = ((SerializableTriggerBehaviour)module.Behaviour).Actions;
		converted.Behaviour.Actions = TriggerBehaviourConverter.ConvertBack(actions, session);
		converted.PassiveScalingOptions = PassiveScalingOptionsConverter.ConvertBack(module.PassiveScalingOptions);
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
	}

	private static void AddModule(Profile profile, SerializableDetectorModule module, ReverseConversionSession session)
	{
		var weights = (DetectorWeights)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		converted.PassiveScalingOptions = PassiveScalingOptionsConverter.ConvertBack(module.PassiveScalingOptions);
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
		converted.ActiveScalingOptions = ActiveScalingOptionsConverter.ConvertBack(module.ActiveScalingOptions);
		converted.ActiveWalkingOptions = ActiveWalkingOptionsConverter.ConvertBack(module.ActiveWalkingOptions);
		SetBehaviour(converted, module.Behaviour, session);
	}

	private static void AddModule(Profile profile, SerializablePoserModule module, ReverseConversionSession session)
	{
		var weights = (PoserWeights)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		converted.PassiveScalingOptions = PassiveScalingOptionsConverter.ConvertBack(module.PassiveScalingOptions);
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
		converted.ActiveScalingOptions = ActiveScalingOptionsConverter.ConvertBack(module.ActiveScalingOptions);
		converted.ActiveWalkingOptions = ActiveWalkingOptionsConverter.ConvertBack(module.ActiveWalkingOptions);
		SetBehaviour(converted, module.Behaviour, session);
	}

	private static void SetBehaviour(Behavioural behavioural, SerializableBehaviour behaviour, ReverseConversionSession session)
	{
		switch (behaviour)
		{
			case SerializableAimAssistBehaviour aimAssistBehaviour:
				SetBehaviour(behavioural, aimAssistBehaviour, session);
				break;
			case SerializableAimBehaviour aimBehaviour:
				SetBehaviour(behavioural, aimBehaviour, session);
				break;
			case SerializableTriggerBehaviour triggerBehaviour:
				SetBehaviour(behavioural, triggerBehaviour, session);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(behaviour));
		}
	}

	private static void SetBehaviour(
		Behavioural behavioural,
		SerializableAimAssistBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetAimAssistBehaviour();
		converted.Tags = AimAssistBehavioursConverter.ConvertBack(behaviour.Tags, session);
		converted.GainFactor = behaviour.GainFactor;
		converted.AttenuationFactor = behaviour.AttenuationFactor;
		converted.DirectionCorrectionFactor = behaviour.DirectionCorrectionFactor;
	}

	private static void SetBehaviour(Behavioural behavioural,
		SerializableAimBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetAimBehaviour();
		converted.Tags = AimBehaviourConverter.ConvertBack(behaviour.Tags, session);
	}

	private static void SetBehaviour(
		Behavioural behavioural,
		SerializableTriggerBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetTriggerBehaviour();
		converted.Actions = TriggerBehaviourConverter.ConvertBack(behaviour.Actions, session);
	}
}