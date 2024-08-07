using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.DataSets.Tags;
using AimAssistBehaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.AimAssistBehaviour;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class AimAssistBehavioursConverter
{
	public static AimAssistBehaviour Convert(Domain.Model.Profiles.Behaviours.AimAssistBehaviour behaviour, ConversionSession session)
	{
		var tags = Convert(behaviour.Tags, session);
		return new AimAssistBehaviour(behaviour, tags);
	}

	public static ImmutableDictionary<Tag, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions> ConvertBack(ImmutableArray<AimAssistBehaviourTagOptions> tags, ReverseConversionSession session)
	{
		return tags.Select(tag => ConvertBack(tag, session)).ToImmutableDictionary();
	}

	private static ImmutableArray<AimAssistBehaviourTagOptions> Convert(
		IReadOnlyDictionary<Tag, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static AimAssistBehaviourTagOptions Convert(
		KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions> tag,
		ConversionSession session)
	{
		var tagId = session.Tags[tag.Key];
		return new AimAssistBehaviourTagOptions(tagId, tag.Value);
	}

	private static KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions> ConvertBack(
		AimAssistBehaviourTagOptions options,
		ReverseConversionSession session)
	{
		return new KeyValuePair<Tag, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions>(
			session.Tags[options.TagId],
			new Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions(options.Priority, options.TargetAreaScale, options.VerticalOffset));
	}
}