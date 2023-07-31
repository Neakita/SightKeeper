using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public Screenshot Screenshot { get; }
    public bool IsAsset => Screenshot.Asset != null;

    public ScreenshotViewModel(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }
}