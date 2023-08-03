using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public Screenshot Item { get; }

    public byte[] Content => Item.Content;
    public bool IsAsset => Item.Asset != null;

    public ScreenshotViewModel(Screenshot item)
    {
        Item = item;
    }

    public void NotifyIsAssetChanged() => OnPropertiesChanged(nameof(IsAsset));
}