using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPointViewModelFactory
{
	public KeyPointViewModel CreateViewModel(PoserItemViewModel item, KeyPoint keyPoint)
	{
		if (keyPoint is KeyPoint3D keyPoint3D)
		{
			var poser3DItemViewModel = (Poser3DItemViewModel)item;
			KeyPoint3DViewModel keyPoint3DViewModel = new(poser3DItemViewModel, keyPoint3D);
			poser3DItemViewModel.AddKeyPoint(keyPoint3DViewModel);
			return keyPoint3DViewModel;
		}
		var poser2DItemViewModel = (Poser2DItemViewModel)item;
		KeyPoint2DViewModel keyPoint2DViewModel = new(poser2DItemViewModel, keyPoint);
		poser2DItemViewModel.AddKeyPoint(keyPoint2DViewModel);
		return keyPoint2DViewModel;
	}
}