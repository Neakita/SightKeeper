using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal sealed class PoserTagsConverter
{
	public PoserTagsConverter(ConversionSession session)
	{
		_session = session;
		_tagsConverter = new TagsConverter(session);
	}

	public IEnumerable<PackablePoserTag> ConvertTags(IEnumerable<PoserTag> tags)
	{
		return tags.Select((tag, index) => ConvertTag(index, tag));
	}

	private readonly ConversionSession _session;

	private PackablePoserTag ConvertTag(int index, PoserTag tag)
	{
		_session.TagsIndexes.Add(tag, (byte)index);
		return new PackablePoserTag
		{
			KeyPointTags = _tagsConverter.ConvertTags(tag.KeyPointTags).ToImmutableArray(),
			Name = tag.Name,
			Color = tag.Color
		};
	}

	private readonly TagsConverter _tagsConverter;
}