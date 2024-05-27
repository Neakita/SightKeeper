using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshotViewModel
{
    public Screenshot Item { get; }

    public Image Image => _screenshotsDataAccess.LoadImage(Item);

    public bool IsAsset => _isAssetSubject.Value;
    public IObservable<bool> IsAssetObservable => _isAssetSubject.AsObservable();

    public DateTime CreationDate => Item.CreationDate;

    public ScreenshotViewModel(ScreenshotsDataAccess screenshotsDataAccess, Screenshot item)
    {
        _screenshotsDataAccess = screenshotsDataAccess;
        Item = item;
        _isAssetSubject = new BehaviorSubject<bool>(item.Asset != null);
    }
    
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private readonly BehaviorSubject<bool> _isAssetSubject;

    public void NotifyIsAssetChanged()
    {
        var isCurrentlyAsset = Item.Asset != null;
        if (_isAssetSubject.Value != isCurrentlyAsset)
            _isAssetSubject.OnNext(isCurrentlyAsset);
    }
}