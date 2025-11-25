using SightKeeper.Application.Misc;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

internal sealed class WrappingFactory<T>(Factory<T> inner, Wrapper<T> wrapper) : Factory<T>
{
	public T Create()
	{
		var obj = inner.Create();
		var wrappedObj = wrapper.Wrap(obj);
		if (obj is PostWrappingInitializable<T> initializable)
			initializable.Initialize(wrappedObj);
		return wrappedObj;
	}
}