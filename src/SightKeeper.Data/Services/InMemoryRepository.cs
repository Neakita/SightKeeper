using System.Reactive.Subjects;

namespace SightKeeper.Data.Services;

internal sealed class InMemoryRepository<T>(Lock editingLock, ChangeListener changeListener) : Repository<T>, ShortcutWriteRepository<T>
{
	public IReadOnlyCollection<T> Items => _items;
	public IObservable<T> Added => _added;
	public IObservable<T> Removed => _removed;

	public void Add(T item)
	{
		lock (editingLock)
		{
			_items.Add(item);
			changeListener.SetDataChanged();
		}
		_added.OnNext(item);
	}

	void ShortcutWriteRepository<T>.Add(T item)
	{
		_items.Add(item);
	}

	public void Remove(T item)
	{
		lock (editingLock)
		{
			_items.Remove(item);
			changeListener.SetDataChanged();
		}
		if (item is DeletableData removableData)
			removableData.DeleteData();
		_removed.OnNext(item);
		if (item is IDisposable disposable)
			disposable.Dispose();
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	private readonly HashSet<T> _items = new();
	private readonly Subject<T> _added = new();
	private readonly Subject<T> _removed = new();
}