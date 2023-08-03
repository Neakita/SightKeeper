using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class DetectorItemViewModel : ViewModel
{
    public DetectorItem? Item
    {
        get => _item;
        set
        {
            if (_item != null)
                ThrowHelper.ThrowInvalidOperationException("Value already set");
            SetProperty(ref _item, value);
        }
    }
    
    public ItemClass ItemClass { get; set; }
    public BoundingBoxViewModel Bounding { get; private set; }

    public DetectorItemViewModel(DetectorItem item)
    {
        Item = item;
        ItemClass = item.ItemClass;
        Bounding = new BoundingBoxViewModel(item.BoundingBox);
    }

    public DetectorItemViewModel(ItemClass itemClass, Point position)
    {
        ItemClass = itemClass;
        Bounding = new BoundingBoxViewModel(position);
    }

    private DetectorItem? _item;
}