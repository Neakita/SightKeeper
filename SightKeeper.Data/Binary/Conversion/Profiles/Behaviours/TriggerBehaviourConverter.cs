﻿using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Behaviours;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class TriggerBehaviourConverter
{
	public static SerializableTriggerBehaviour Convert(TriggerBehaviour behaviour, ConversionSession session)
	{
		var actions = Convert(behaviour.Actions, session);
		return new SerializableTriggerBehaviour(actions);
	}

	public static ImmutableDictionary<Tag, Action> ConvertBack(
		ImmutableArray<SerializableAction> actions,
		ReverseConversionSession session)
	{
		return actions.Select(action => ConvertBack(action, session)).ToImmutableDictionary();
	}

	private static ImmutableArray<SerializableAction> Convert(
		IReadOnlyDictionary<Tag, Action> actions,
		ConversionSession session)
	{
		return actions.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableAction Convert(
		KeyValuePair<Tag, Action> action,
		ConversionSession session)
	{
		var tagId = session.Tags[action.Key];
		return new SerializableAction(tagId);
	}

	private static KeyValuePair<Tag, Action> ConvertBack(SerializableAction action, ReverseConversionSession session)
	{
		throw new NotImplementedException();
	}
}