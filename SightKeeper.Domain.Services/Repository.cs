namespace SightKeeper.Domain.Services;

public interface Repository<TItem>
{
	IReadOnlyCollection<TItem> Items { get; }

	TItem Get(int id);
	bool Contains(TItem item);
	void Add(TItem item);
	void Remove(TItem item);
}
