using System;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class DrawerItemsFactory
{
	public BoundedItemViewModel CreateItemViewModel(DetectorItem item) => item switch
	{
		PoserItem poserItem => new PoserItemViewModel(poserItem),
		not null => new DetectorItemViewModel(item),
		_ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
	};
}