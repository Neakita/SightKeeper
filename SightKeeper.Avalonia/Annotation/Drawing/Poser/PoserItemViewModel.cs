using System.Collections.Generic;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class PoserItemViewModel : DrawerItemViewModel
{
	public abstract override PoserItem Item { get; }
	public abstract override PoserTag Tag { get; }
	public abstract IReadOnlyList<KeyPointViewModel> KeyPoints { get; }

	protected PoserItemViewModel(BoundingEditor boundingEditor) : base(boundingEditor)
	{
	}
}