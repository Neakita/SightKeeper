namespace SightKeeper.Application;

public interface WriteDataAccess<in T>
{
	void Add(T item);
	void Remove(T item);
}