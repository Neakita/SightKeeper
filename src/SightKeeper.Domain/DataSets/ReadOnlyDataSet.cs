namespace SightKeeper.Domain.DataSets;

public interface ReadOnlyDataSet<out TTag, out TAsset>
{
	IEnumerable<TTag> Tags { get; }
	IEnumerable<TAsset> Assets { get; }
}