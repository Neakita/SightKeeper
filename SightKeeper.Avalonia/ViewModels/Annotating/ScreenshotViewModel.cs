using System;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public Screenshot Item { get; }

    public Task<Image> Image => _imageLoader.LoadAsync(Item);

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