using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

public sealed class ReadOnlyDataSetValue<TAsset> : ReadOnlyDataSet<TAsset>
{
	public IEnumerable<ReadOnlyTag> Tags { get; }
	public IEnumerable<TAsset> Assets { get; }

	public ReadOnlyDataSetValue(IEnumerable<ReadOnlyTag> tags, IEnumerable<TAsset> assets)
	{
		Tags = tags;
		Assets = assets;
	}
}