namespace SightKeeper.Data.Services;

internal sealed class CompositeWrapper<T>(IReadOnlyCollection<Wrapper<T>> wrappers) : Wrapper<T>
{
	public T Wrap(T obj)
	{
		foreach (var wrapper in wrappers)
			obj = wrapper.Wrap(obj);
		return obj;
	}
}