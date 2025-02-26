using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

public sealed class ScreenshotsLibraryViewModel : ViewModel
{
	public ImageSet Value { get; }
	public string Name => Value.Name;
	public Image? PreviewScreenshot => Value.Images.RandomOrDefault();

	public ScreenshotsLibraryViewModel(ImageSet value)
	{
		Value = value;
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}