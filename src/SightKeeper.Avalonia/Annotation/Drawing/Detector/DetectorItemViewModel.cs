using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

public sealed class DetectorItemViewModel : BoundedItemViewModel
{
	public override DetectorItem Value { get; }
	public override Tag Tag => Value.Tag;

	public DetectorItemViewModel(DetectorItem item)
	{
		Value = item;
		Bounding = Value.Bounding;
	}
}