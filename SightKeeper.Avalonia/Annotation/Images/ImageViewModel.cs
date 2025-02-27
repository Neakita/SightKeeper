using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed class ImageViewModel : ViewModel
{
	public Image Value { get; }

	public ImageViewModel(Image value)
	{
		Value = value;
	}
}