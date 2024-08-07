using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviours;
using AimAssistBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.AimAssistBehaviour;
using AimBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.AimBehaviour;
using Behaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.Behaviour;
using Profile = SightKeeper.Data.Binary.Profiles.Profile;
using TriggerBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.TriggerBehaviour;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ProfilesConverter
{
	public ProfilesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_modulesConverter = new ModulesConverter(weightsDataAccess);
	}
	
	public ImmutableArray<Profile> Convert(IEnumerable<Domain.Model.Profiles.Profile> profiles, ConversionSession session)
	{
		return profiles.Select(profile => Convert(profile, session)).ToImmutableArray();
	}

	public HashSet<Domain.Model.Profiles.Profile> ConvertBack(ImmutableArray<Profile> profiles, ReverseConversionSession session)
	{
		return profiles.Select(profile => ConvertBack(profile, session)).ToHashSet();
	}

	private readonly ModulesConverter _modulesConverter;

	private Profile Convert(Domain.Model.Profiles.Profile profile, ConversionSession session)
	{
		var modules = _modulesConverter.Convert(profile.Modules, session);
		return new Profile(profile.Name, modules);
	}

	private Domain.Model.Profiles.Profile ConvertBack(Profile profile, ReverseConversionSession session)
	{
		Domain.Model.Profiles.Profile converted = new(profile.Name);
		foreach (var module in profile.Modules)
			AddModule(converted, module, session);
		return converted;
	}

	private static void AddModule(Domain.Model.Profiles.Profile profile, Module module, ReverseConversionSession session)
	{
		switch (module)
		{
			case ClassifierModule classifierModule:
				AddModule(profile, classifierModule, session);
				break;
			case DetectorModule detectorModule:
				AddModule(profile, detectorModule, session);
				break;
			case PoserModule poserModule:
				AddModule(profile, poserModule, session);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(module));
		}
	}

	private static void AddModule(Domain.Model.Profiles.Profile profile, ClassifierModule module, ReverseConversionSession session)
	{
		var weights = (Weights<ClassifierTag>)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		var actions = ((TriggerBehaviour)module.Behaviour).Actions;
		converted.Behaviour.Actions = TriggerBehaviourConverter.ConvertBack(actions, session);
		converted.PassiveScalingOptions = module.PassiveScalingOptions?.Convert();
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
	}

	private static void AddModule(Domain.Model.Profiles.Profile profile, DetectorModule module, ReverseConversionSession session)
	{
		var weights = (Weights<DetectorTag>)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		converted.PassiveScalingOptions = module.PassiveScalingOptions?.Convert();
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
		converted.ActiveScalingOptions = ActiveScalingOptionsConverter.ConvertBack(module.ActiveScalingOptions);
		converted.ActiveWalkingOptions = ActiveWalkingOptionsConverter.ConvertBack(module.ActiveWalkingOptions);
		SetBehaviour(converted, module.Behaviour, session);
	}

	private static void AddModule(Domain.Model.Profiles.Profile profile, PoserModule module, ReverseConversionSession session)
	{
		var weights = (Weights<Poser2DTag, KeyPointTag2D>)session.Weights[module.WeightsId];
		var converted = profile.CreateModule(weights);
		converted.PassiveScalingOptions = module.PassiveScalingOptions?.Convert();
		converted.PassiveWalkingOptions = PassiveWalkingOptionsConverter.ConvertBack(module.PassiveWalkingOptions);
		converted.ActiveScalingOptions = ActiveScalingOptionsConverter.ConvertBack(module.ActiveScalingOptions);
		converted.ActiveWalkingOptions = ActiveWalkingOptionsConverter.ConvertBack(module.ActiveWalkingOptions);
		SetBehaviour(converted, module.Behaviour, session);
	}

	private static void SetBehaviour(Behavioural behavioural, Behaviour behaviour, ReverseConversionSession session)
	{
		switch (behaviour)
		{
			case AimAssistBehaviour aimAssistBehaviour:
				SetBehaviour(behavioural, aimAssistBehaviour, session);
				break;
			case AimBehaviour aimBehaviour:
				SetBehaviour(behavioural, aimBehaviour, session);
				break;
			case TriggerBehaviour triggerBehaviour:
				SetBehaviour(behavioural, triggerBehaviour, session);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(behaviour));
		}
	}

	private static void SetBehaviour(
		Behavioural behavioural,
		AimAssistBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetAimAssistBehaviour();
		converted.Tags = AimAssistBehavioursConverter.ConvertBack(behaviour.Tags, session);
		converted.GainFactor = behaviour.GainFactor;
		converted.AttenuationFactor = behaviour.AttenuationFactor;
		converted.DirectionCorrectionFactor = behaviour.DirectionCorrectionFactor;
	}

	private static void SetBehaviour(Behavioural behavioural,
		AimBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetAimBehaviour();
		converted.Tags = AimBehaviourConverter.ConvertBack(behaviour.Tags, session);
	}

	private static void SetBehaviour(
		Behavioural behavioural,
		TriggerBehaviour behaviour,
		ReverseConversionSession session)
	{
		var converted = behavioural.SetTriggerBehaviour();
		converted.Actions = TriggerBehaviourConverter.ConvertBack(behaviour.Actions, session);
	}
}