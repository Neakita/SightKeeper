namespace SightKeeper.Domain.Model.DataSets;

internal sealed class ItemClassNameComparer : IComparer<ItemClass>
{
	public static ItemClassNameComparer Instance { get; } = new();

	public int Compare(ItemClass? x, ItemClass? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
	}
}