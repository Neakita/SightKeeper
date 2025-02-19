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
		{
			var poser3DItemViewModel = (Poser3DItemViewModel)item;
			KeyPoint3DViewModel keyPoint3DViewModel = new(_annotator, poser3DItemViewModel, keyPoint3D);
			poser3DItemViewModel.AddKeyPoint(keyPoint3DViewModel);
			return keyPoint3DViewModel;
		}
		var poser2DItemViewModel = (Poser2DItemViewModel)item;
		KeyPoint2DViewModel keyPoint2DViewModel = new(_annotator, poser2DItemViewModel, keyPoint);
		poser2DItemViewModel.AddKeyPoint(keyPoint2DViewModel);
		return keyPoint2DViewModel;
	}

	private readonly PoserAnnotator _annotator;
}