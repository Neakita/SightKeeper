using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class CropTrainData<TTag, TAsset>(
	ReadOnlyDataSet<TTag, TAsset> inner,
	CropRectanglesProvider<TAsset> cropRectanglesProvider,
	AssetCropper<TAsset> assetCropper)
	: ReadOnlyDataSet<TTag, TAsset>
{
	public IEnumerable<TTag> Tags => inner.Tags;

	public IEnumerable<TAsset> Assets => inner.Assets.SelectMany(CropToItems);

	private IEnumerable<TAsset> CropToItems(TAsset asset)
	{
		foreach (var cropRectangle in cropRectanglesProvider.GetCropRectangles(asset))
			yield return assetCropper.CropAsset(asset, cropRectangle);
	}
}