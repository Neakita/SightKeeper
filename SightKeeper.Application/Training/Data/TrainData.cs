namespace SightKeeper.Application.Training.Data;

public interface TrainData<out TAsset>
{
	IEnumerable<TagData> Tags { get; }
	IEnumerable<TAsset> Assets { get; }
}