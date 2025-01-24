using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

public sealed class DetectorItemViewModel : DrawerItemViewModel
{
	public override DetectorItem Item { get; }
	public override Tag Tag => Item.Tag;

	public DetectorItemViewModel(DetectorItem item, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Item = item;
		Bounding = Item.Bounding;
	}
}