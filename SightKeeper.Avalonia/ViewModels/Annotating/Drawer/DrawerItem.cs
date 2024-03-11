namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface DrawerItem
{
    bool IsDashed { get; }
    BoundingViewModel Bounding { get; }
    Tag Tag { get; }
}