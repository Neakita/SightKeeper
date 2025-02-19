using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPointViewModelFactory
{
	public KeyPointViewModelFactory(PoserAnnotator annotator)
	{
		_annotator = annotator;
	}

	public KeyPointViewModel CreateViewModel(PoserItemViewModel item, KeyPoint keyPoint)
	{
		if (keyPoint is KeyPoint3D keyPoint3D)
			return new KeyPoint3DViewModel(_annotator, (Poser3DItemViewModel)item, keyPoint3D);
		return new KeyPoint2DViewModel(_annotator, (Poser2DItemViewModel)item, keyPoint);
	}

	private readonly PoserAnnotator _annotator;
}