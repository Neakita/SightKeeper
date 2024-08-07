using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal static class TagsConverter
{
	public static ImmutableArray<SerializableTag> Convert(IReadOnlyCollection<Tag> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableTag Convert(Tag tag, ConversionSession session)
	{
		SerializableTag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}
}