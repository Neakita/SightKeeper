using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class DesignImageSetCardDataContext : ImageSetCardDataContext
{
	public string Name { get; }
	public Image? PreviewImage => null;
	public ICommand EditCommand => new RelayCommand(() => { });
	public ICommand DeleteCommand => new RelayCommand(() => { });

	public DesignImageSetCardDataContext(string name)
	{
		Name = name;
	}
}