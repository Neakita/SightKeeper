using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Conversion;

internal static class TagsConverter
{
	public static ImmutableArray<SerializableTag> Convert(IReadOnlyCollection<Tag> tags, ConversionSession session)
	{
		var converted = tags.Select(Convert).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static SerializableTag Convert(Tag tag)
	{
		return new SerializableTag(Id.Create(), tag);
	}
}