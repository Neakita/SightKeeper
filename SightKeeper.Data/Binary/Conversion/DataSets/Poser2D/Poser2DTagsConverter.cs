using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using Poser2DTag = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DTag;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal static class Poser2DTagsConverter
{
	public static ImmutableArray<Poser2DTag> Convert(IReadOnlyCollection<Domain.Model.DataSets.Poser2D.Poser2DTag> tags, ConversionSession session)
	{
		var converted = tags.Select(tag => Convert(tag, session)).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static Poser2DTag Convert(Domain.Model.DataSets.Poser2D.Poser2DTag tag, ConversionSession session)
	{
		Poser2DTag converted = new(
			Id.Create(),
			tag,
			Convert(tag.KeyPoints, session),
			Convert(tag.Properties));
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<Tag> Convert(IEnumerable<KeyPointTag2D> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static Tag Convert(KeyPointTag2D tag, ConversionSession session)
	{
		Tag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<Binary.DataSets.Poser.NumericItemProperty> Convert(IEnumerable<NumericItemProperty> properties)
	{
		return properties.Select(Binary.DataSets.Poser.NumericItemProperty.Create).ToImmutableArray();
	}
}