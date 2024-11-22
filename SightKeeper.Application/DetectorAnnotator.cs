using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application;

public abstract class DetectorAnnotator
{
	public virtual DetectorItem CreateItem(Screenshot<DetectorAsset> screenshot, DetectorTag tag, Bounding bounding)
	{
		var asset = screenshot.Asset ?? screenshot.MakeAsset();
		return asset.CreateItem(tag, bounding);
	}

	public virtual void SetBounding(DetectorItem item, Bounding bounding)
	{
		item.Bounding = bounding;
	}
}