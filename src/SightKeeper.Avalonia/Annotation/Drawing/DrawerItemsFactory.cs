using System;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class DrawerItemsFactory
{
	public BoundedItemViewModel CreateItemViewModel(AssetItem item) => item switch
	{
		DetectorItem detectorItem => new DetectorItemViewModel(detectorItem),
		PoserItem poserItem => new PoserItemViewModel(poserItem),
		_ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
	};
}