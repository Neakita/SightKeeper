using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public Screenshot Item { get; }

    public Task<byte[]> Content
    {
        get
        {
            return _imageLoader.LoadAsync(Item).ToObservable().Select(item => item.Content).ToTask();
        }
    }

    public bool IsAsset => Item.Asset != null;

    public DateTime CreationDate => Item.CreationDate;

    public ScreenshotViewModel(ScreenshotImageLoader imageLoader, Screenshot item)
    {
        _imageLoader = imageLoader;
        Item = item;
    }
    
    private readonly ScreenshotImageLoader _imageLoader;

    public void NotifyIsAssetChanged() => OnPropertiesChanged(nameof(IsAsset));
}