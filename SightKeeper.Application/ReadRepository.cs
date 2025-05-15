namespace SightKeeper.Application;

public interface ReadRepository<out T>
{
	IReadOnlyCollection<T> Items { get; }
}