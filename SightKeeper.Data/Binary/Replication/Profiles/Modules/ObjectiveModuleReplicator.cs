using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal abstract class ObjectiveModuleReplicator : ModuleReplicator
{
	public override ObjectiveModule Replicate(Profile profile, PackableModule packedModule, ReplicationSession session)
	{
		var module = (ObjectiveModule)base.Replicate(profile, packedModule, session);
		var typedPackedModule = (PackableObjectiveModule)packedModule;
		module.ActiveScalingOptions = ConvertActiveScalingOptions(typedPackedModule.ActiveScalingOptions);
		module.ActiveWalkingOptions = ConvertActiveWalkingOptions(typedPackedModule.ActiveWalkingOptions);
		ReplicateBehavior(module, typedPackedModule.Behavior, session);
		return module;
	}

	protected abstract override ObjectiveModule CreateModule(Profile profile, Weights weights);

	private static ActiveScalingOptions? ConvertActiveScalingOptions(PackableActiveScalingOptions? options) => options switch
	{
		null => null,
		PackableAdaptiveScalingOptions adaptiveScalingOptions => new AdaptiveScalingOptions
		{
			Margin = adaptiveScalingOptions.Margin,
			MaximumScaling = adaptiveScalingOptions.MaximumScaling
		},
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};

	private static ActiveWalkingOptions? ConvertActiveWalkingOptions(PackableActiveWalkingOptions? options) => options switch
	{
		null => null,
		PackableTrackingWalkingOptions trackingWalkingOptions => new TrackingWalkingOptions(trackingWalkingOptions.MaximumOffset),
		_ => throw new ArgumentOutOfRangeException(nameof(options))
	};

	private static void ReplicateBehavior(ObjectiveModule module, PackableBehavior packedBehavior, ReplicationSession session)
	{
		Behavior _ = packedBehavior switch
		{
			PackableTriggerBehavior triggerBehavior => ReplicateTriggerBehavior(module, triggerBehavior),
			PackableAimBehavior aimBehavior => ReplicateAimBehavior(module, aimBehavior, session),
			PackableAimAssistBehavior aimAssistBehavior => ReplicateAimAssistBehavior(module, aimAssistBehavior, session),
			_ => throw new ArgumentOutOfRangeException(nameof(packedBehavior))
		};
	}

	private static TriggerBehavior ReplicateTriggerBehavior(ObjectiveModule module, PackableTriggerBehavior packedBehavior)
	{
		var behavior = module.SetBehavior<TriggerBehavior>();
		// TODO actions
		return behavior;
	}

	private static AimBehavior ReplicateAimBehavior(
		ObjectiveModule module,
		PackableAimBehavior packedAimBehavior,
		ReplicationSession session)
	{
		Guard.IsNotNull(session.Tags);
		var behavior = module.SetBehavior<AimBehavior>();
		behavior.Tags = packedAimBehavior.Tags.ToImmutableDictionary(
			pair => session.Tags[(module.Weights.DataSet, pair.Key)],
			pair => ReplicateTagOptions(pair.Value));
		return behavior;
		static AimBehavior.TagOptions ReplicateTagOptions(PackableAimBehaviorTagOptions options)
		{
			return new AimBehavior.TagOptions(options.Priority, options.VerticalOffset);
		}
	}

	private static AimAssistBehavior ReplicateAimAssistBehavior(
		ObjectiveModule module,
		PackableAimAssistBehavior packedAimBehavior,
		ReplicationSession session)
	{
		Guard.IsNotNull(session.Tags);
		var behavior = module.SetBehavior<AimAssistBehavior>();
		behavior.Tags = packedAimBehavior.Tags.ToImmutableDictionary(
			pair => session.Tags[(module.Weights.DataSet, pair.Key)],
			pair => ReplicateTagOptions(pair.Value));
		return behavior;
		static AimAssistBehavior.TagOptions ReplicateTagOptions(PackableAimAssistBehaviorTagOptions options)
		{
			return new AimAssistBehavior.TagOptions(options.Priority, options.TargetAreaScale, options.VerticalOffset);
		}
	}
}