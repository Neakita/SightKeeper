using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class DesignImageSetCardDataContext : ImageSetCardDataContext
{
	public string Name { get; }
	public ImageDataContext? PreviewImage => null;
	public ICommand EditCommand => new RelayCommand(() => { });
	public ICommand DeleteCommand => new RelayCommand(() => { });

	public DesignImageSetCardDataContext(string name)
	{
		Name = name;
	}
}