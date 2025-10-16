namespace SightKeeper.Data.Services;

internal interface ShortcutWriteRepository<in T>
{
	void Add(T item);
}