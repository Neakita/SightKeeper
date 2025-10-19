namespace SightKeeper.Data.Services;

internal sealed class FuncWrapper<TDecorator, T>(Func<T, TDecorator> func) : Wrapper<T>
	where TDecorator : T
{
	public T Wrap(T obj)
	{
		return func(obj);
	}
}