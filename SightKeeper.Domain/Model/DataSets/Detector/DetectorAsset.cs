using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>, AssetsFactory<DetectorAsset>, AssetsDestroyer<DetectorAsset>
{
	public static DetectorAsset Create(Screenshot<DetectorAsset> screenshot)
	{
		DetectorAsset asset = new(screenshot, screenshot.DataSet.AssetsLibrary);
		screenshot.SetAsset(asset);
		return asset;
	}

	public static void Destroy(DetectorAsset asset)
	{
		asset.Screenshot.SetAsset(null);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}

	public override Screenshot<DetectorAsset> Screenshot { get; }
	public override AssetsLibrary<DetectorAsset> Library { get; }
	public override DataSet DataSet => Library.DataSet;
	
    public DetectorItem CreateItem(DetectorTag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding, this);
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

    private DetectorAsset(Screenshot<DetectorAsset> screenshot, AssetsLibrary<DetectorAsset> library)
    {
	    Screenshot = screenshot;
	    Library = library;
    }
}