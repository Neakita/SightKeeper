using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal interface CropRectanglesProvider<in TAsset>
{
	IEnumerable<Rectangle> GetCropRectangles(TAsset asset);
}