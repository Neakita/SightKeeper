using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorItemViewModel : DrawerItemViewModel
{
	public DetectorItem Value { get; }
	public override Tag Tag => Value.Tag;
	public override Bounding Bounding { get; set; }

	public DetectorItemViewModel(DetectorItem value)
	{
		Value = value;
	}
}