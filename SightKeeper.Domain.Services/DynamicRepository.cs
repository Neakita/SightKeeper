using DynamicData;

namespace SightKeeper.Domain.Services;

public interface DynamicRepository<TItem> : Repository<TItem>
{
	ISourceCache<TItem, int> ItemsCache { get; }
}
