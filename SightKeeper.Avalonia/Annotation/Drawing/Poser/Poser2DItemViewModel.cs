using System.Collections.Generic;
using System.Collections.ObjectModel;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser2DItemViewModel : PoserItemViewModel
{
	public override Poser2DItem Item { get; }
	public override PoserTag Tag => Item.Tag;

	public override IReadOnlyList<KeyPointViewModel> KeyPoints { get; } = ReadOnlyCollection<KeyPointViewModel>.Empty;

	public Poser2DItemViewModel(Poser2DItem item, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Item = item;
	}
}