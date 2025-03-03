using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class KeyPointViewModel : ViewModel, KeyPointDataContext, DrawerItemDataContext
{
	public abstract PoserItemViewModel Item { get; }
	public abstract KeyPoint Value { get; }
	public abstract Tag Tag { get; }

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

	protected KeyPointViewModel(PoserAnnotator annotator)
	{
		_annotator = annotator;
	}

	private readonly PoserAnnotator _annotator;
}