namespace SightKeeper.Application;

public interface ReadDataAccess<out T>
{
	IReadOnlyCollection<T> Items { get; }
}