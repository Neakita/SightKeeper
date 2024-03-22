namespace SightKeeper.Domain.Model.DataSets;

internal sealed class ScreenshotsDateComparer : IComparer<Screenshot>
{
	public static ScreenshotsDateComparer Instance { get; } = new();
	
	public int Compare(Screenshot? x, Screenshot? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return x.CreationDate.CompareTo(y.CreationDate);
	}
}