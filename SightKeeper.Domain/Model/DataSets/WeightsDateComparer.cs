using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Model.DataSets;

internal sealed class WeightsDateComparer : IComparer<DetectorWeights>
{
	public static WeightsDateComparer Instance { get; } = new();
	
	public int Compare(DetectorWeights? x, DetectorWeights? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return x.CreationDate.CompareTo(y.CreationDate);
	}
}