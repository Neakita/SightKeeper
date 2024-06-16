namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>
{
	public DetectorAssetsLibrary Library { get; }
	public DetectorDataSet DataSet => Library.DataSet;
	
    public DetectorItem CreateItem(DetectorTag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding);
        AddItem(item);
        return item;
    }

    internal DetectorAsset(Screenshot screenshot, DetectorAssetsLibrary library) : base(screenshot)
    {
	    Library = library;
    }
}