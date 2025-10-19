namespace SightKeeper.Data.Services;

internal interface Wrapper<T>
{
	T Wrap(T obj);
}