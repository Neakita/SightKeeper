namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>
{
	public override DetectorScreenshot Screenshot { get; }
	public override DetectorAssetsLibrary Library { get; }
	public override DetectorDataSet DataSet => Library.DataSet;
	
    public DetectorItem CreateItem(DetectorTag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding);
        tag.AddItem(item);
        AddItem(item);
        return item;
    }

    public override void DeleteItem(DetectorItem item)
    {
	    base.DeleteItem(item);
	    item.Tag.RemoveItem(item);
    }

    public override void ClearItems()
    {
	    foreach (var item in Items)
		    item.Tag.RemoveItem(item);
	    base.ClearItems();
    }

    internal DetectorAsset(DetectorScreenshot screenshot, DetectorAssetsLibrary library)
    {
	    Screenshot = screenshot;
	    Library = library;
    }
}