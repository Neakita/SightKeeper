namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>
{
	public DetectorScreenshot Screenshot { get; }
	public DetectorAssetsLibrary Library { get; }
	public DetectorDataSet DataSet => Library.DataSet;
	
    public DetectorItem CreateItem(DetectorTag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding);
        AddItem(item);
        return item;
    }

    internal DetectorAsset(DetectorScreenshot screenshot, DetectorAssetsLibrary library) : base(screenshot)
    {
	    Screenshot = screenshot;
	    Library = library;
    }
}