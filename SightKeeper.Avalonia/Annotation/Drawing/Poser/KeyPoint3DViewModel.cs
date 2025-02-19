using System;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint3DViewModel : KeyPointViewModel
{
	public override KeyPoint Value => throw new NotImplementedException();
	public override Tag Tag => throw new NotImplementedException();
	public override Poser3DItemViewModel Item => throw new NotImplementedException();

	public KeyPoint3DViewModel(PoserAnnotator annotator) : base(annotator)
	{
	}
}