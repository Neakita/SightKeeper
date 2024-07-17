using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class AimBehaviourConverter
{
	public static SerializableAimBehaviour Convert(AimBehaviour behaviour, ConversionSession session)
	{
		var tags = Convert(behaviour.Tags, session);
		return new SerializableAimBehaviour(tags);
	}

	private static ImmutableArray<SerializableAimBehaviourTagOptions> Convert(
		IReadOnlyDictionary<Tag, AimBehaviour.TagOptions> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableAimBehaviourTagOptions Convert(
		KeyValuePair<Tag, AimBehaviour.TagOptions> tag,
		ConversionSession session)
	{
		var tagId = session.Tags[tag.Key];
		return new SerializableAimBehaviourTagOptions(tagId, tag.Value);
	}
}