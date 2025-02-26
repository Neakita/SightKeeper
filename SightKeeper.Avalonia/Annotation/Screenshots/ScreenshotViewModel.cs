using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public sealed class ScreenshotViewModel : ViewModel
{
	public Image Value { get; }
	public IReadOnlyCollection<Asset> Assets => Value.Assets;

	public ScreenshotViewModel(Image value)
	{
		Value = value;
	}

	internal void NotifyAssetsChanged()
	{
		OnPropertyChanged(nameof(Assets));
	}
}