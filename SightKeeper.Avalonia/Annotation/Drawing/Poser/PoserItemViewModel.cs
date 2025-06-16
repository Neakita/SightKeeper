using System.Collections.Generic;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class PoserItemViewModel : BoundedItemViewModel
{
	public abstract override DomainPoserItem Value { get; }
	public abstract override DomainPoserTag Tag { get; }
	public abstract IReadOnlyList<KeyPointViewModel> KeyPoints { get; }

	internal abstract void RemoveKeyPoint(KeyPointViewModel keyPoint);

	protected PoserItemViewModel(BoundingEditor boundingEditor) : base(boundingEditor)
	{
	}
}