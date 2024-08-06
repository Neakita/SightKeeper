using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal static class Poser2DTagsConverter
{
	public static ImmutableArray<SerializablePoser2DTag> Convert(IReadOnlyCollection<Poser2DTag> tags, ConversionSession session)
	{
		var converted = tags.Select(tag => Convert(tag, session)).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static SerializablePoser2DTag Convert(Poser2DTag tag, ConversionSession session)
	{
		SerializablePoser2DTag converted = new(Id.Create(), tag, Convert(tag.KeyPoints, session));
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<SerializableTag> Convert(IEnumerable<KeyPointTag2D> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableTag Convert(KeyPointTag2D tag, ConversionSession session)
	{
		SerializableTag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}
}