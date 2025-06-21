using System.Collections;
using System.Reactive.Disposables;
using Vibrance.Changes;

namespace SightKeeper.Data;

internal sealed class ExternalObservableCollection<T> : ReadOnlyObservableCollection<T>
{
	public int Count
	{
		get
		{
			ObjectDisposedException.ThrowIf(_isDisposed, this);
			return _inner.Count;
		}
	}

	public bool HasObservers
	{
		get
		{
			ObjectDisposedException.ThrowIf(_isDisposed, this);
			return _observers.Count > 0;
		}
	}

	public ExternalObservableCollection(IReadOnlyCollection<T> inner)
	{
		_inner = inner;
	}

	public IEnumerator<T> GetEnumerator()
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		return _inner.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		return GetEnumerator();
	}

	public IDisposable Subscribe(IObserver<Change<T>> observer)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		_observers.Add(observer);
		SendInitialItems(observer);
		return Disposable.Create((_observers, observer), static tuple => tuple._observers.Remove(tuple.observer));
	}

	public void Notify(Change<T> change)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		foreach (var observer in _observers)
			observer.OnNext(change);
	}

	public void Dispose()
	{
		if (_isDisposed)
			return;
		_observers.Clear();
		_isDisposed = true;
	}

	private readonly IReadOnlyCollection<T> _inner;
	private readonly List<IObserver<Change<T>>> _observers = new();
	private bool _isDisposed;

	private void SendInitialItems(IObserver<Change<T>> observer)
	{
		if (Count == 0)
			return;
		Addition<T> change = new()
		{
			Items = this.ToList()
		};
		observer.OnNext(change);
	}
}