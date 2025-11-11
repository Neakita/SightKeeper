namespace SightKeeper.Application.Misc;

public interface Factory<out T>
{
	T Create();
}