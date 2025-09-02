using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data;

internal sealed class ExternalObservableList<T> : ReadOnlyObservableList<T>
{
#pragma warning disable 67
	public event NotifyCollectionChangedEventHandler? CollectionChanged;
	public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67

	public int Count
	{
		get
		{
			ObjectDisposedException.ThrowIf(_isDisposed, this);
			return _inner.Count;
		}
	}

	public T this[int index]
	{
		get
		{
			ObjectDisposedException.ThrowIf(_isDisposed, this);
			return _inner[index];
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

	public ExternalObservableList(IReadOnlyList<T> inner)
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

	public IDisposable Subscribe(IObserver<IndexedChange<T>> observer)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		_observers.Add(observer);
		SendInitialItems(observer);
		return Disposable.Create((_observers, observer), static tuple => tuple._observers.Remove(tuple.observer));
	}

	public void Notify(IndexedChange<T> change)
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

	private readonly IReadOnlyList<T> _inner;
	private readonly List<IObserver<IndexedChange<T>>> _observers = new();
	private bool _isDisposed;

	private void SendInitialItems(IObserver<IndexedChange<T>> observer)
	{
		if (Count == 0)
			return;
		Insertion<T> change = new()
		{
			Index = 0,
			Items = this.ToList()
		};
		observer.OnNext(change);
	}
}