namespace SightKeeper.Application.Training.Data;

public sealed class TrainDataValue<TAsset> : TrainData<TAsset>
{
	public IEnumerable<TagData> Tags { get; }
	public IEnumerable<TAsset> Assets { get; }

	public TrainDataValue(IEnumerable<TagData> tags, IEnumerable<TAsset> assets)
	{
		Tags = tags;
		Assets = assets;
	}
}