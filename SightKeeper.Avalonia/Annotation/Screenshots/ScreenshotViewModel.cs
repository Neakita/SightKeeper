using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class ScreenshotViewModel : ViewModel
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