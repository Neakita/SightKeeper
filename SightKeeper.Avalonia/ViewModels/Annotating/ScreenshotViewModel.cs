using System;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel : ViewModel
{
    public Screenshot Item { get; }

    public byte[] Content
    {
        get
        {
            _imageLoader.Load(Item);
            return Item.Image.Content;
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