using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data;

internal sealed class ReadOnlyDataSetValue<TTag, TAsset> : ReadOnlyDataSet<TTag, TAsset>
{
	public IEnumerable<TTag> Tags { get; }
	public IEnumerable<TAsset> Assets { get; }

	public ReadOnlyDataSetValue(IEnumerable<TTag> tags, IEnumerable<TAsset> assets)
	{
		Tags = tags;
		Assets = assets;
	}
}