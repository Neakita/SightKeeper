namespace SightKeeper.Application.Training.Data.Transforming;

public interface TrainDataTransformer<TAsset>
{
	TrainData<TAsset> Transform(TrainData<TAsset> data);
}