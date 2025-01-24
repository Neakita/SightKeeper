using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DesignDrawerItemViewModel : DrawerItemDataContext
{
	public Tag Tag
	{
		get
		{
			DetectorDataSet dataSet = new();
			return dataSet.TagsLibrary.CreateTag("Cop");
		}
	}

	public Bounding Bounding { get; set; } = new(0.1, 0.1, 0.9, 0.9);
}