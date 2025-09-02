namespace SightKeeper.Application;

public interface WriteRepository<in T>
{
	void Add(T set);
	void Remove(T set);
}