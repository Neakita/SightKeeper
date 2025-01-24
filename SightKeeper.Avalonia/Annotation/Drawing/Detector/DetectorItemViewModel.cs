using SightKeeper.Application.Annotation;
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
		set => SetProperty(Value.Bounding, value, this,
			(viewModel, bounding) => viewModel._boundingEditor.SetBounding(viewModel.Value, bounding));
	}

	public DetectorItemViewModel(DetectorItem value, BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
		Value = value;
		Bounding = Value.Bounding;
	}

	private readonly BoundingEditor _boundingEditor;
}