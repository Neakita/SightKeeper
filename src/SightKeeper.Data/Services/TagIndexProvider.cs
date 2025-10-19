using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Services;

internal interface TagIndexProvider
{
	byte GetTagIndex(Tag tag);
}