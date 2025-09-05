using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

public interface TrainDataTransformer<TTag, TAsset>
{
	ReadOnlyDataSet<TTag, TAsset> Transform(ReadOnlyDataSet<TTag, TAsset> data);
}