using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public abstract class DetectorAnnotator
{
	public virtual DetectorItem CreateItem(AssetsLibrary<DetectorAsset> assetsLibrary, Screenshot screenshot, Tag tag, Bounding bounding)
	{
		var asset = assetsLibrary.GetOrMakeAsset(screenshot);
		return asset.CreateItem(tag, bounding);
	}

	public virtual void SetBounding(DetectorItem item, Bounding bounding)
	{
		item.Bounding = bounding;
	}
}