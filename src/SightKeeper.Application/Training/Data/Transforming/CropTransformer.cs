using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class CropTransformer<TAsset>(CropRectanglesProvider<TAsset> cropRectanglesProvider, AssetCropper<TAsset> cropper) : TrainDataTransformer<TAsset>
{
	public ReadOnlyDataSet<TAsset> Transform(ReadOnlyDataSet<TAsset> data)
	{
		return new CropTrainData<TAsset>(data, cropRectanglesProvider, cropper);
	}
}