namespace SightKeeper.Domain.Model.DataSets.Weights;

internal sealed class WeightsDateComparer : IComparer<Weights>
{
	public static WeightsDateComparer Instance { get; } = new();
	
	public int Compare(Weights? x, Weights? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return x.CreationDate.CompareTo(y.CreationDate);
	}
}