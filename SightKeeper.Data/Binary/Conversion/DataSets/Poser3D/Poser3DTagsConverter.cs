using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DTagsConverter
{
	public static ImmutableArray<SerializablePoser3DTag> Convert(IReadOnlyCollection<Poser3DTag> tags, ConversionSession session)
	{
		var converted = tags.Select(tag => Convert(tag, session)).ToImmutableArray();
		foreach (var (tag, convertedTag) in tags.Zip(converted))
			session.Tags.Add(tag, convertedTag.Id);
		return converted;
	}

	private static SerializablePoser3DTag Convert(Poser3DTag tag, ConversionSession session)
	{
		SerializablePoser3DTag converted = new(
			Id.Create(),
			tag,
			Convert(tag.KeyPoints, session),
			Convert(tag.NumericProperties),
			Convert(tag.BooleanProperties));
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<SerializableTag> Convert(IEnumerable<KeyPointTag3D> tags, ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private static SerializableTag Convert(KeyPointTag3D tag, ConversionSession session)
	{
		SerializableTag converted = new(Id.Create(), tag);
		session.Tags.Add(tag, converted.Id);
		return converted;
	}

	private static ImmutableArray<SerializableNumericItemProperty> Convert(IEnumerable<NumericItemProperty> properties)
	{
		return properties.Select(SerializableNumericItemProperty.Create).ToImmutableArray();
	}

	private static ImmutableArray<string> Convert(IEnumerable<BooleanItemProperty> properties)
	{
		return properties.Select(property => property.Name).ToImmutableArray();
	}
}