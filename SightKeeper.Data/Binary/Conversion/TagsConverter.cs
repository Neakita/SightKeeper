using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class TagsConverter
{
	public TagsConverter(ConversionSession session)
	{
		_session = session;
	}

	public IEnumerable<PackableTag> ConvertTags(IEnumerable<Tag> tags)
	{
		return tags.Select((tag, index) => ConvertTag(index, tag));
	}

	private readonly ConversionSession _session;

	private PackableTag ConvertTag(int index, Tag tag)
	{
		Guard.IsLessThanOrEqualTo(index, byte.MaxValue);
		var id = (byte)index;
		_session.TagsIds.Add(tag, id);
		return new PackableTag
		{
			Id = id,
			Name = tag.Name,
			Color = tag.Color
		};
	}
}