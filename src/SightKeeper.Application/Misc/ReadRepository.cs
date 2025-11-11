namespace SightKeeper.Application.Misc;

public interface ReadRepository<out T>
{
	IReadOnlyCollection<T> Items { get; }
}