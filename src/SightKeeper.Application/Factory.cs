namespace SightKeeper.Application;

public interface Factory<out T>
{
	T Create();
}