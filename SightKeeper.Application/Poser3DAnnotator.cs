using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public abstract class Poser3DAnnotator
{
	public virtual Poser3DItem CreateItem(AssetsLibrary<Poser3DAsset> assetsLibrary, Screenshot screenshot, PoserTag tag, Bounding bounding)
	{
		var asset = assetsLibrary.GetOrMakeAsset(screenshot);
		return asset.CreateItem(tag, bounding);
	}

	public virtual void SetBounding(DetectorItem item, Bounding bounding)
	{
		item.Bounding = bounding;
	}
}