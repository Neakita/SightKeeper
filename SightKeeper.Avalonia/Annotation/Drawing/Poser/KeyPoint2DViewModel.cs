using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint2DViewModel : KeyPointViewModel
{
	public override Poser2DItemViewModel Item { get; }
	public override DomainKeyPoint Value { get; }
	public override DomainTag Tag => Value.Tag;

	public KeyPoint2DViewModel(PoserAnnotator annotator, Poser2DItemViewModel item, DomainKeyPoint value) : base(annotator)
	{
		Item = item;
		Value = value;
	}
}