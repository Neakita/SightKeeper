using System;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint3DViewModel : KeyPointViewModel
{
	public override Tag Tag => throw new NotImplementedException();

	public override PoserItemViewModel Item => throw new NotImplementedException();

	public override Vector2<double> Position
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}