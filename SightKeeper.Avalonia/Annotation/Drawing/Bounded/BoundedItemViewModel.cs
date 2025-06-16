using Avalonia.Media;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public abstract class BoundedItemViewModel : ViewModel, BoundedItemDataContext
{
	public abstract DomainTag Tag { get; }
	public abstract DomainBoundedItem Value { get; }

	public Bounding Bounding
	{
		get => Value.Bounding;
		set
		{
			if (SetProperty(Bounding, value, this,
				(viewModel, bounding) => viewModel._boundingEditor.SetBounding(viewModel.Value, bounding)))
				OnPropertyChanged(nameof(Position));
		}
	}

	public Vector2<double> Position => Bounding.Position;
	public string Name => Tag.Name;
	public Color Color => Color.FromUInt32(Tag.Color);

	protected BoundedItemViewModel(BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
	}

	private readonly BoundingEditor _boundingEditor;
}