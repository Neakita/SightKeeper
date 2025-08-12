using System.Collections;
using System.Reactive.Disposables;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Tests.Annotation;

internal sealed class FakeAssets : IReadOnlyCollection<Asset>, IObservable<object>
{
	public IEnumerator<Asset> GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int Count => throw new NotImplementedException();

	public IDisposable Subscribe(IObserver<object> observer)
	{
		_observer = observer;
		return Disposable.Empty;
	}

	public void NotifyObserver()
	{
		_observer?.OnNext(new object());
	}

	private IObserver<object>? _observer;
}