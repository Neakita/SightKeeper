using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal abstract class ObjectiveModuleConverter : ModuleConverter
{
	protected sealed override PackableObjectiveModule CreateModule(
		Module module,
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		ConversionSession session)
	{
		var typedModule = (ObjectiveModule)module;
		var activeScalingOptions = ConvertActiveScalingOptions(typedModule.ActiveScalingOptions);
		var activeWalkingOptions = ConvertActiveWalkingOptions(typedModule.ActiveWalkingOptions);
		var behavior = ConvertBehavior(typedModule.Behavior, session);
		return CreateModule(
			typedModule,
			weightsId,
			passiveScalingOptions,
			passiveWalkingOptions,
			activeScalingOptions,
			activeWalkingOptions,
			behavior);
	}

	protected abstract PackableObjectiveModule CreateModule(
		ObjectiveModule module,
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions,
		PackableActiveScalingOptions? activeScalingOptions,
		PackableActiveWalkingOptions? activeWalkingOptions,
		PackableBehavior behavior);

	private static PackableActiveScalingOptions? ConvertActiveScalingOptions(ActiveScalingOptions? options) => options switch
	{
		null => null,
		AdaptiveScalingOptions adaptiveScalingOptions => new PackableAdaptiveScalingOptions(adaptiveScalingOptions.Margin, adaptiveScalingOptions.MaximumScaling),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};
	
	private static PackableActiveWalkingOptions? ConvertActiveWalkingOptions(ActiveWalkingOptions? options) => options switch
	{
		null => null,
		TrackingWalkingOptions trackingWalkingOptions => new PackableTrackingWalkingOptions(trackingWalkingOptions.MaximumOffset),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};

	private static PackableBehavior ConvertBehavior(Behavior behavior, ConversionSession session) => behavior switch
	{
		TriggerBehavior triggerBehavior => ConvertTriggerBehavior(triggerBehavior),
		AimBehavior aimBehavior => ConvertAimBehavior(aimBehavior, session),
		AimAssistBehavior aimAssistBehavior => ConvertAimAssistBehavior(aimAssistBehavior, session),
		_ => throw new ArgumentOutOfRangeException(nameof(behavior))
	};

	private static PackableAimBehavior ConvertAimBehavior(AimBehavior behavior, ConversionSession session)
	{
		return new PackableAimBehavior(ConvertTags(behavior.Tags));
		ImmutableArray<PackableAimBehaviorTagOptions> ConvertTags(
			IEnumerable<AimBehavior.TagOptions> tagsOptions) =>
			tagsOptions.Select(ConvertOptions).ToImmutableArray();
		PackableAimBehaviorTagOptions ConvertOptions(AimBehavior.TagOptions options) =>
			new(session.TagsIds[options.Tag], options.Priority, options.VerticalOffset);
	}

	private static PackableAimAssistBehavior ConvertAimAssistBehavior(AimAssistBehavior behavior, ConversionSession session)
	{
		return new PackableAimAssistBehavior(
			ConvertTags(behavior.Tags),
			behavior.DirectionCorrectionFactor,
			behavior.GainFactor,
			behavior.AttenuationFactor);
		ImmutableDictionary<byte, PackableAimAssistBehaviorTagOptions> ConvertTags(
			ImmutableDictionary<Tag, AimAssistBehavior.TagOptions> tags) => 
			tags.ToImmutableDictionary(
				pair => session.TagsIds[pair.Key],
				pair => ConvertOptions(pair.Value));
		static PackableAimAssistBehaviorTagOptions ConvertOptions(AimAssistBehavior.TagOptions options) =>
			new(options.Priority, options.TargetAreaScale, options.VerticalOffset);
	}
}