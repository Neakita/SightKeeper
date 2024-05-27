using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal interface DrawerItem
{
    bool IsDashed { get; }
    BoundingViewModel Bounding { get; }
    ItemClass ItemClass { get; }
}