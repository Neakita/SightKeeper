using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public IObservable<bool> IsAssetChanged => _isAssetChanged.AsObservable();
    public Screenshot Screenshot { get; }
    public bool IsAsset => Screenshot.Asset != null;

    public ScreenshotViewModel(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }

    public void NotifyIsAssetChanged()
    {
        _isAssetChanged.OnNext(IsAsset);
        OnPropertyChanged(nameof(IsAsset));
    }

    private readonly Subject<bool> _isAssetChanged = new();
}