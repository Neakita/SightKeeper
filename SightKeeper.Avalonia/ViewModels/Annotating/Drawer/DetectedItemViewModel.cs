using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class DetectedItemViewModel : ViewModel, DrawerItem
{
    public bool IsDashed => true;
    public BoundingViewModel Bounding { get; }
    public ItemClass ItemClass { get; }
    public float Probability { get; }

    public DetectedItemViewModel(BoundingViewModel bounding, ItemClass itemClass,  float probability)
    {
        Bounding = bounding;
        ItemClass = itemClass;
        Probability = probability;
    }
}