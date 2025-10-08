using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal interface AssetCropper<TAsset>
{
	TAsset CropAsset(TAsset asset, Rectangle cropRectangle);
}