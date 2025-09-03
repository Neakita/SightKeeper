using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

public interface TrainDataTransformer<TAsset>
{
	ReadOnlyDataSet<TAsset> Transform(ReadOnlyDataSet<TAsset> data);
}