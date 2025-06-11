namespace SightKeeper.Domain;

public interface Decorator<out T>
{
	T Inner { get; }
}