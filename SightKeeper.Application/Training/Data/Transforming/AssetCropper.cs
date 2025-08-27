using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

public interface AssetCropper<TAsset>
{
	TAsset CropAsset(TAsset asset, Rectangle cropRectangle);
}