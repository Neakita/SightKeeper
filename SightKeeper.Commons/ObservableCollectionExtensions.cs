namespace SightKeeper.Commons;

public static class ObservableCollectionExtensions
{
	public static int RemoveAll<T>(this IList<T> collection, Func<T, bool> predicate)
	{
		var indexes = collection
			.WithIndexes()
			.Where(pair => predicate(pair.item))
			.SelectIndex()
			.OrderDescending()
			.ToList();
		foreach (var index in indexes)
			collection.RemoveAt(index);
		return indexes.Count;
	}
}