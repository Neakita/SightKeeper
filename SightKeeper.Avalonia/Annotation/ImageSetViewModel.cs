using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class ImageSetViewModel : ViewModel, ImageSetDataContext
{
	public DomainImageSet Value { get; }
	public string Name => Value.Name;

	public ImageSetViewModel(DomainImageSet value)
	{
		Value = value;
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}