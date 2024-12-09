using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DesignDrawerItemViewModel : DrawerItemViewModel
{
	public override Tag Tag
	{
		get
		{
			DetectorDataSet dataSet = new();
			return dataSet.TagsLibrary.CreateTag("Cop");
		}
	}

	public override Bounding Bounding { get; set; } = new(0.1, 0.1, 0.9, 0.9);
}