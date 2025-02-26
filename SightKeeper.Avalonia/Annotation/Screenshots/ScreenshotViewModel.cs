using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public sealed class ScreenshotViewModel : ViewModel
{
	public Screenshot Value { get; }
	public IReadOnlyCollection<Asset> Assets => Value.Assets;

	public ScreenshotViewModel(Screenshot value)
	{
		Value = value;
	}

	internal void NotifyAssetsChanged()
	{
		OnPropertyChanged(nameof(Assets));
	}
}