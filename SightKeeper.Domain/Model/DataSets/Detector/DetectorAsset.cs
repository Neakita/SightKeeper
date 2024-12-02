﻿using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>
{
	public override Screenshot<DetectorAsset> Screenshot { get; }
	public override DetectorAssetsLibrary Library { get; }
	public override DetectorDataSet DataSet => Library.DataSet;
	
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

    internal DetectorAsset(Screenshot<DetectorAsset> screenshot, DetectorAssetsLibrary library)
    {
	    Screenshot = screenshot;
	    Library = library;
    }
}