using System.Collections.Immutable;
using FlakeId;
using Tag = SightKeeper.Data.Binary.DataSets.Tag;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal static class TagsConverter
{
	public static ImmutableArray<Tag> Convert(IReadOnlyCollection<Domain.Model.DataSets.Tags.Tag> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static Tag Convert(Domain.Model.DataSets.Tags.Tag tag, ConversionSession session)
	{
		Tag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}
}