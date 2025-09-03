using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPointViewModelFactory
{
	public KeyPointViewModel CreateViewModel(PoserItemViewModel item, KeyPoint keyPoint)
	{
		KeyPointViewModel keyPoint2DViewModel = new(item, keyPoint);
		item.AddKeyPoint(keyPoint2DViewModel);
		return keyPoint2DViewModel;
	}
}