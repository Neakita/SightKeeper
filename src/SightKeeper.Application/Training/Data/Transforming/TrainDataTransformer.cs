using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

internal interface TrainDataTransformer<TTag, TAsset>
{
	ReadOnlyDataSet<TTag, TAsset> Transform(ReadOnlyDataSet<TTag, TAsset> data);
}