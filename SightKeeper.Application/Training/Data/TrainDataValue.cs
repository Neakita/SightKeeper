using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

public sealed class TrainDataValue<TAsset> : TrainData<TAsset>
{
	public IEnumerable<ReadOnlyTag> Tags { get; }
	public IEnumerable<TAsset> Assets { get; }

	public TrainDataValue(IEnumerable<ReadOnlyTag> tags, IEnumerable<TAsset> assets)
	{
		Tags = tags;
		Assets = assets;
	}
}