using Avalonia.Media;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class KeyPointViewModel : ViewModel, KeyPointDataContext, DrawerItemDataContext
{
	public abstract PoserItemViewModel Item { get; }
	public abstract DomainKeyPoint Value { get; }
	public abstract DomainTag Tag { get; }

	public Vector2<double> Position
	{
		get => Value.Position;
		set
		{
			if (Position == value)
				return;
			OnPropertyChanging();
			_annotator.SetKeyPointPosition(Value, value);
			OnPropertyChanged();
		}
	}
	public string Name => Tag.Name;
	public Color Color => Color.FromUInt32(Tag.Color);

	protected KeyPointViewModel(PoserAnnotator annotator)
	{
		_annotator = annotator;
	}

	private readonly PoserAnnotator _annotator;
}