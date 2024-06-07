namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>
{
    public DetectorItem CreateItem(DetectorTag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding);
        AddItem(item);
        return item;
    }

    internal DetectorAsset(Screenshot screenshot) : base(screenshot)
    {
    }
}