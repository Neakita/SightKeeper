using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel
{
    public Screenshot Item { get; }

    public byte[] ImageData => _screenshotsDataAccess.LoadImage(Item);

    public bool IsAsset => _isAssetSubject.Value;
    public IObservable<bool> IsAssetObservable => _isAssetSubject.AsObservable();

    public DateTime CreationDate => Item.CreationDate;

    public ScreenshotViewModel(ScreenshotsDataAccess screenshotsDataAccess, Screenshot item)
    {
        _screenshotsDataAccess = screenshotsDataAccess;
        Item = item;
        var asset = item.Asset;
        _isAssetSubject = new BehaviorSubject<bool>(asset != null);
    }
    
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private readonly BehaviorSubject<bool> _isAssetSubject;

    public void NotifyIsAssetChanged()
    {
	    var asset = Item.Asset;
        var isCurrentlyAsset = asset != null;
        if (_isAssetSubject.Value != isCurrentlyAsset)
            _isAssetSubject.OnNext(isCurrentlyAsset);
    }
}