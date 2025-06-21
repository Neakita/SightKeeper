using System.Collections.ObjectModel;
using Vibrance.Changes;

namespace SightKeeper.Data;

public class Removal<T> : Change<T>
{
	public required IReadOnlyList<T> Items { get; init; }

	IReadOnlyList<T> Change<T>.OldItems => Items;
	IReadOnlyList<T> Change<T>.NewItems => ReadOnlyCollection<T>.Empty;
}