using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal interface DrawerItem
{
    bool IsDashed { get; }
    BoundingViewModel Bounding { get; }
    Tag Tag { get; }
}