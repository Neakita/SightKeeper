using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using NumericItemProperty = SightKeeper.Data.Binary.DataSets.Poser.NumericItemProperty;
using Poser3DTag = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DTag;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DTagsConverter
{
	public static ImmutableArray<Poser3DTag> Convert(IReadOnlyCollection<Domain.Model.DataSets.Poser3D.Poser3DTag> tags, ConversionSession session)
	{
		var converted = tags.Select(tag => Convert(tag, session)).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static Poser3DTag Convert(Domain.Model.DataSets.Poser3D.Poser3DTag tag, ConversionSession session)
	{
		Poser3DTag converted = new(
			Id.Create(),
			tag,
			Convert(tag.KeyPoints, session),
			Convert(tag.NumericProperties),
			Convert(tag.BooleanProperties));
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<Tag> Convert(IEnumerable<KeyPointTag3D> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static Tag Convert(KeyPointTag3D tag, ConversionSession session)
	{
		Tag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<NumericItemProperty> Convert(IEnumerable<Domain.Model.DataSets.Poser.NumericItemProperty> properties)
	{
		return properties.Select(NumericItemProperty.Create).ToImmutableArray();
	}

	private static ImmutableArray<string> Convert(IEnumerable<BooleanItemProperty> properties)
	{
		return properties.Select(property => property.Name).ToImmutableArray();
	}
}