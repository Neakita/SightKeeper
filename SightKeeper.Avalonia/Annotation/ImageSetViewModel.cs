using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class ImageSetViewModel : ViewModel, ImageSetDataContext
{
	public ImageSet Value { get; }
	public string Name => Value.Name;

	public ImageSetViewModel(ImageSet value)
	{
		Value = value;
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}