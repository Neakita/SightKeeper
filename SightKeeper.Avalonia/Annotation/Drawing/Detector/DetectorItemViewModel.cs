using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorItemViewModel : DrawerItemViewModel
{
	public DetectorItem Value { get; }
	public override DetectorTag Tag => Value.Tag;

	public override Bounding Bounding
	{
		get => Value.Bounding;
		set => _annotator.SetBounding(Value, value);
	}

	public DetectorItemViewModel(DetectorItem value, DetectorAnnotator annotator)
	{
		_annotator = annotator;
		Value = value;
		Bounding = Value.Bounding;
	}

	private readonly DetectorAnnotator _annotator;
}