using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.DataSets.Tags;
using AimBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.AimBehaviour;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class AimBehaviourConverter
{
	public static AimBehaviour Convert(Domain.Model.Profiles.Behaviours.AimBehaviour behaviour, ConversionSession session)
	{
		var tags = Convert(behaviour.Tags, session);
		return new AimBehaviour(tags);
	}

	public static ImmutableDictionary<Tag, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions> ConvertBack(
		ImmutableArray<AimBehaviourTagOptions> tags,
		ReverseConversionSession session)
	{
		return tags.Select(tag => ConvertBack(tag, session)).ToImmutableDictionary();
	}

	private static ImmutableArray<AimBehaviourTagOptions> Convert(
		IReadOnlyDictionary<Tag, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static AimBehaviourTagOptions Convert(
		KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions> tag,
		ConversionSession session)
	{
		var tagId = session.Tags[tag.Key];
		return new AimBehaviourTagOptions(tagId, tag.Value);
	}

	private static KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions> ConvertBack(
		AimBehaviourTagOptions options,
		ReverseConversionSession session)
	{
		return new KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions>(
			session.Tags[options.TagId],
			new Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions(options.Priority, options.VerticalOffset));
	}
}