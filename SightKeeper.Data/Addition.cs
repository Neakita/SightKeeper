using System.Collections.ObjectModel;
using Vibrance.Changes;

namespace SightKeeper.Data;

public class Addition<T> : Change<T>
{
	public required IReadOnlyList<T> Items
	{
		get;
		init
		{
			if (value.Count == 0)
				throw new ArgumentException($"{nameof(value)} for {nameof(Items)} expected to have at least one item");
			field = value;
		}
	}

	IReadOnlyList<T> Change<T>.OldItems => ReadOnlyCollection<T>.Empty;
	IReadOnlyList<T> Change<T>.NewItems => Items;
}