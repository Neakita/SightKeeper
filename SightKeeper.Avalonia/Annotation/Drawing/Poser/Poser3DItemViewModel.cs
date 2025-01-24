using System;
using System.Collections.Generic;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser3DItemViewModel : PoserItemViewModel
{
	public override Poser3DItem Item { get; }
	public override PoserTag Tag => Item.Tag;
	public override IReadOnlyList<KeyPointViewModel> KeyPoints => throw new NotImplementedException();

	public Poser3DItemViewModel(Poser3DItem item, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Item = item;
	}
}