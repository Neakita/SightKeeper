using SightKeeper.Data.DataSets.Tags.Decorators;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Services;

internal sealed class DecoratorTagIndexProvider : TagIndexProvider
{
	public byte GetTagIndex(Tag tag)
	{
		var indexHolder = tag.GetFirst<TagIndexHolder>();
		return indexHolder.Index;
	}
}