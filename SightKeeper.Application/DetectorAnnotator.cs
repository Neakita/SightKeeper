using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public class DetectorAnnotator
{
	public virtual DetectorItem CreateItem(Screenshot screenshot, Tag tag, Bounding bounding)
	{
		throw new NotImplementedException();
	}

	public virtual void SetBounding(DetectorItem item, Bounding bounding)
	{
		item.Bounding = bounding;
	}
}