namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class CropTrainData<TAsset>(
	TrainData<TAsset> inner,
	CropRectanglesProvider<TAsset> cropRectanglesProvider,
	AssetCropper<TAsset> assetCropper)
	: TrainData<TAsset>
{
	public IEnumerable<TagData> Tags => inner.Tags;

	public IEnumerable<TAsset> Assets => inner.Assets.SelectMany(CropToItems);

	private IEnumerable<TAsset> CropToItems(TAsset asset)
	{
		foreach (var cropRectangle in cropRectanglesProvider.GetCropRectangles(asset))
			yield return assetCropper.CropAsset(asset, cropRectangle);
	}
}