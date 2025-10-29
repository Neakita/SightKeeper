using SightKeeper.Domain;

namespace SightKeeper.Data.Services;

internal sealed class CompositeWrapper<T>(IReadOnlyCollection<Wrapper<T>> wrappers) : Wrapper<T>
	where T : notnull
{
	public T Wrap(T obj)
	{
		foreach (var wrapper in wrappers)
			obj = wrapper.Wrap(obj);
		Initialize(obj);
		return obj;
	}

	private static void Initialize(T obj)
	{
		var initializables = obj.Get<PostWrappingInitializable<T>>();
		foreach (var initializable in initializables)
			initializable.Initialize(obj);
	}
}