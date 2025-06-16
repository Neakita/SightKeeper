using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

public sealed class DetectorItemViewModel : BoundedItemViewModel
{
	public override DetectorItem Value { get; }
	public override DomainTag Tag => Value.Tag;

	public DetectorItemViewModel(DetectorItem item, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Value = item;
		Bounding = Value.Bounding;
	}
}