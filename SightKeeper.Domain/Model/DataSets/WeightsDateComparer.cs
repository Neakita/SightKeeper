namespace SightKeeper.Domain.Model.DataSets;

internal sealed class WeightsDateComparer<TWeights> : IComparer<TWeights> where TWeights : Weights
{
	public static WeightsDateComparer<TWeights> Instance { get; } = new();
	
	public int Compare(TWeights? x, TWeights? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return x.CreationDate.CompareTo(y.CreationDate);
	}
}