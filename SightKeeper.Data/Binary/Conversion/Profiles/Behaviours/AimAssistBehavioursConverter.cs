using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class AimAssistBehavioursConverter
{
	public static SerializableAimAssistBehaviour Convert(AimAssistBehaviour behaviour, ConversionSession session)
	{
		var tags = Convert(behaviour.Tags, session);
		return new SerializableAimAssistBehaviour(behaviour, tags);
	}

	private static ImmutableArray<SerializableAimAssistBehaviourTagOptions> Convert(
		IReadOnlyDictionary<Tag, AimAssistBehaviour.TagOptions> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableAimAssistBehaviourTagOptions Convert(
		KeyValuePair<Tag, AimAssistBehaviour.TagOptions> tag,
		ConversionSession session)
	{
		var tagId = session.Tags[tag.Key];
		return new SerializableAimAssistBehaviourTagOptions(tagId, tag.Value);
	}
}