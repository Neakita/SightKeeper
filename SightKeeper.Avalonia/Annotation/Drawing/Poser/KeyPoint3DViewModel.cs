using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint3DViewModel : KeyPointViewModel
{
	public override Poser3DItemViewModel Item { get; }
	public override DomainKeyPoint3D Value { get; }
	public override DomainTag Tag => Value.Tag;

	public bool IsVisible
	{
		get => Value.IsVisible;
		set
		{
			OnPropertyChanging();
			_annotator.SetKeyPointVisibility(Value, value);
			OnPropertyChanged();
		}
	}

	public KeyPoint3DViewModel(PoserAnnotator annotator, Poser3DItemViewModel item, DomainKeyPoint3D value) : base(annotator)
	{
		_annotator = annotator;
		Item = item;
		Value = value;
	}

	private readonly PoserAnnotator _annotator;
}