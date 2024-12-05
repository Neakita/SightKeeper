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
		var id = (byte)index;
		_session.TagsIds.Add(tag, id);
		return new PackablePoserTag
		{
			KeyPointTags = _tagsConverter.ConvertTags(tag.KeyPointTags).ToImmutableArray(),
			Id = id,
			Name = tag.Name,
			Color = tag.Color
		};
	}

	private readonly TagsConverter _tagsConverter;
}