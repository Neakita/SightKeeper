using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

public interface CropRectanglesProvider<in TAsset>
{
	IEnumerable<Rectangle> GetCropRectangles(TAsset asset);
}