using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

public sealed class DetectorItemViewModel : DrawerItemViewModel
{
	public DetectorItem Value { get; }
	public override Tag Tag => Value.Tag;

	public override Bounding Bounding
	{
		get => Value.Bounding;
		set => _boundingEditor.SetBounding(Value, value);
	}

	public DetectorItemViewModel(DetectorItem value, BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
		Value = value;
		Bounding = Value.Bounding;
	}

	private readonly BoundingEditor _boundingEditor;
}