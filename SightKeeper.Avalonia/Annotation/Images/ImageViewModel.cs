using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed class ImageViewModel : ViewModel
{
	public Image Value { get; }
	public IReadOnlyCollection<Asset> Assets => Value.Assets;

	public ImageViewModel(Image value)
	{
		Value = value;
	}

	internal void NotifyAssetsChanged()
	{
		OnPropertyChanged(nameof(Assets));
	}
}