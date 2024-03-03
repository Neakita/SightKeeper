using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface DrawerItem
{
    bool IsDashed { get; }
    BoundingViewModel Bounding { get; }
    ItemClass ItemClass { get; }
}