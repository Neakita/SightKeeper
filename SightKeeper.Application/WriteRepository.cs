namespace SightKeeper.Application;

public interface WriteRepository<in T>
{
	void Add(T item);
	void Remove(T item);
}