using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel
{
    public Screenshot Item { get; }

    public Task<Image> Image => _imageLoader.LoadAsync(Item);

    public bool IsAsset => _isAssetSubject.Value;
    public IObservable<bool> IsAssetObservable => _isAssetSubject.AsObservable();

    public DateTime CreationDate => Item.CreationDate;

    public ScreenshotViewModel(ScreenshotImageLoader imageLoader, Screenshot item)
    {
        _imageLoader = imageLoader;
        Item = item;
        _isAssetSubject = new BehaviorSubject<bool>(item.Asset != null);
    }
    
    private readonly ScreenshotImageLoader _imageLoader;
    private readonly BehaviorSubject<bool> _isAssetSubject;

    public void NotifyIsAssetChanged()
    {
        var isCurrentlyAsset = Item.Asset != null;
        if (_isAssetSubject.Value != isCurrentlyAsset)
            _isAssetSubject.OnNext(isCurrentlyAsset);
    }
}