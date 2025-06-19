namespace SightKeeper.Data;

internal interface Decorator<out T>
{
	T Inner { get; }
}