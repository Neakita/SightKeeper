using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class CropTransformer<TTag, TAsset>(CropRectanglesProvider<TAsset> cropRectanglesProvider, AssetCropper<TAsset> cropper) : TrainDataTransformer<TTag, TAsset>
{
	public ReadOnlyDataSet<TTag, TAsset> Transform(ReadOnlyDataSet<TTag, TAsset> data)
	{
		return new CropTrainData<TTag, TAsset>(data, cropRectanglesProvider, cropper);
	}
}