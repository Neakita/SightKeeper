using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint3DViewModel : KeyPointViewModel
{
	public override Poser3DItemViewModel Item { get; }
	public override KeyPoint3D Value { get; }
	public override Tag Tag => Value.Tag;

	public KeyPoint3DViewModel(PoserAnnotator annotator, Poser3DItemViewModel item, KeyPoint3D value) : base(annotator)
	{
		Item = item;
		Value = value;
	}
}