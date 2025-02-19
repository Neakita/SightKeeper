using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public abstract class BoundedItemViewModel : ViewModel, BoundedItemDataContext
{
	public abstract Tag Tag { get; }
	public abstract BoundedItem Item { get; }

	public Bounding Bounding
	{
		get => Item.Bounding;
		set
		{
			if (SetProperty(Bounding, value, this,
				(viewModel, bounding) => viewModel._boundingEditor.SetBounding(viewModel.Item, bounding)))
				OnPropertyChanged(nameof(Position));
		}
	}

	public Vector2<double> Position => Bounding.Position;

	protected BoundedItemViewModel(BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
	}

	private readonly BoundingEditor _boundingEditor;
}