using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors.Actions;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Actions;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal abstract class ObjectiveModuleReplicator : ModuleReplicator
{
	public override ObjectiveModule Replicate(Profile profile, PackableModule packedModule)
	{
		var module = (ObjectiveModule)base.Replicate(profile, packedModule);
		var typedPackedModule = (PackableObjectiveModule)packedModule;
		module.ActiveScalingOptions = ConvertActiveScalingOptions(typedPackedModule.ActiveScalingOptions);
		module.ActiveWalkingOptions = ConvertActiveWalkingOptions(typedPackedModule.ActiveWalkingOptions);
		ReplicateBehavior(module, typedPackedModule.Behavior);
		return module;
	}

	protected ObjectiveModuleReplicator(ReplicationSession session) : base(session)
	{
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

	private void ReplicateBehavior(ObjectiveModule module, PackableBehavior packedBehavior)
	{
		Behavior _ = packedBehavior switch
		{
			PackableTriggerBehavior triggerBehavior => ReplicateTriggerBehavior(module, triggerBehavior),
			PackableAimBehavior aimBehavior => ReplicateAimBehavior(module, aimBehavior),
			PackableAimAssistBehavior aimAssistBehavior => ReplicateAimAssistBehavior(module, aimAssistBehavior),
			_ => throw new ArgumentOutOfRangeException(nameof(packedBehavior))
		};
	}

	private TriggerBehavior ReplicateTriggerBehavior(ObjectiveModule module, PackableTriggerBehavior packedBehavior)
	{
		var behavior = module.SetBehavior<TriggerBehavior>();
		behavior.Actions = packedBehavior.Actions.Select(ReplicateActionPair).ToImmutableDictionary();
		return behavior;
		KeyValuePair<Tag, Action> ReplicateActionPair(PackableAction packedAction)
		{
			var tag = Session.Tags[(module.Weights.DataSet, packedAction.Tag)];
			var action = ReplicateAction(packedAction);
			return new KeyValuePair<Tag, Action>(tag, action);
		}
		Action ReplicateAction(PackableAction action)
		{
			return action switch
			{
				PackablePressKeyAction pressKey => new PressKeyAction(pressKey.Type, pressKey.KeyCode),
				_ => throw new ArgumentOutOfRangeException(nameof(action))
			};
		}
	}

	private AimBehavior ReplicateAimBehavior(
		ObjectiveModule module,
		PackableAimBehavior packedAimBehavior)
	{
		Guard.IsNotNull(Session.Tags);
		var behavior = module.SetBehavior<AimBehavior>();
		behavior.Tags = packedAimBehavior.TagsOptions
			.Select(ReplicateTagOptions)
			.ToImmutableArray();
		return behavior;
		AimBehavior.TagOptions ReplicateTagOptions(PackableAimBehaviorTagOptions options)
		{
			return new AimBehavior.TagOptions(Session.Tags[(module.Weights.DataSet, options.TagId)], options.Priority, options.VerticalOffset);
		}
	}

	private AimAssistBehavior ReplicateAimAssistBehavior(
		ObjectiveModule module,
		PackableAimAssistBehavior packedAimBehavior)
	{
		Guard.IsNotNull(Session.Tags);
		var behavior = module.SetBehavior<AimAssistBehavior>();
		behavior.Tags = packedAimBehavior.Tags
			.Select(ReplicateTagOptions)
			.ToImmutableArray();
		return behavior;
		AimAssistBehavior.TagOptions ReplicateTagOptions(PackableAimAssistBehaviorTagOptions options)
		{
			return new AimAssistBehavior.TagOptions(Session.Tags[(module.Weights.DataSet, options.TagId)], options.Priority, options.TargetAreaScale, options.VerticalOffset);
		}
	}
}