using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public abstract class DrawerItemViewModel : ViewModel, DrawerItemDataContext
{
	public abstract Tag Tag { get; }
	public abstract BoundedItem Item { get; }

	public Bounding Bounding
	{
		get => Item.Bounding;
		set => SetProperty(Bounding, value, this,
			(viewModel, bounding) => viewModel._boundingEditor.SetBounding(viewModel.Item, bounding));
	}

	protected DrawerItemViewModel(BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
	}

	private readonly BoundingEditor _boundingEditor;
}