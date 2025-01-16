using System;
using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser3DItemViewModel : PoserItemViewModel
{
	public override PoserTag Tag => Value.Tag;
	public override IReadOnlyList<KeyPointViewModel> KeyPoints => throw new NotImplementedException();
	public Poser3DItem Value { get; }

	public override Bounding Bounding
	{
		get => Value.Bounding;
		set => throw new NotImplementedException();
	}

	public Poser3DItemViewModel(Poser3DItem value)
	{
		Value = value;
	}
}