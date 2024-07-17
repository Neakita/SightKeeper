using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal static class PoserTagsConverter
{
	public static ImmutableArray<SerializablePoserTag> Convert(IReadOnlyCollection<PoserTag> tags, ConversionSession session)
	{
		var converted = tags.Select(tag => Convert(tag, session)).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static SerializablePoserTag Convert(PoserTag tag, ConversionSession session)
	{
		SerializablePoserTag converted = new(Id.Create(), tag, Convert(tag.KeyPoints, session));
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<SerializableTag> Convert(IEnumerable<KeyPointTag> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableTag Convert(KeyPointTag tag, ConversionSession session)
	{
		SerializableTag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}
}