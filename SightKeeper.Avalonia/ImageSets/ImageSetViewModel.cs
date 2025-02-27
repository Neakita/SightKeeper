using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets;

public sealed class ImageSetViewModel : ViewModel
{
	public ImageSet Value { get; }
	public string Name => Value.Name;
	public Image? PreviewImage => Value.Images.RandomOrDefault();

	public ImageSetViewModel(ImageSet value)
	{
		Value = value;
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}