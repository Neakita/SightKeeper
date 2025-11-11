namespace SightKeeper.Application.Misc;

public interface WriteRepository<in T>
{
	void Add(T set);
	void Remove(T set);
}