using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorItemViewModel : DrawerItemViewModel
{
	public DetectorItem Value { get; }
	public override DetectorTag Tag => Value.Tag;
	public override Bounding Bounding => Value.Bounding;

	public DetectorItemViewModel(DetectorItem value)
	{
		Value = value;
	}
}