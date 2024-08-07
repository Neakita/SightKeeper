using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Tags;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;
using TriggerBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.TriggerBehaviour;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class TriggerBehaviourConverter
{
	public static TriggerBehaviour Convert(Domain.Model.Profiles.Behaviours.TriggerBehaviour behaviour, ConversionSession session)
	{
		var actions = Convert(behaviour.Actions, session);
		return new TriggerBehaviour(actions);
	}

	public static ImmutableDictionary<Tag, Action> ConvertBack(
		ImmutableArray<Binary.Profiles.Modules.Behaviours.Action> actions,
		ReverseConversionSession session)
	{
		return actions.Select(action => ConvertBack(action, session)).ToImmutableDictionary();
	}

	private static ImmutableArray<Binary.Profiles.Modules.Behaviours.Action> Convert(
		IReadOnlyDictionary<Tag, Action> actions,
		ConversionSession session)
	{
		return actions.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static Binary.Profiles.Modules.Behaviours.Action Convert(
		KeyValuePair<Tag, Action> action,
		ConversionSession session)
	{
		var tagId = session.Tags[action.Key];
		return new Binary.Profiles.Modules.Behaviours.Action(tagId);
	}

	private static KeyValuePair<Tag, Action> ConvertBack(Binary.Profiles.Modules.Behaviours.Action action, ReverseConversionSession session)
	{
		throw new NotImplementedException();
	}
}