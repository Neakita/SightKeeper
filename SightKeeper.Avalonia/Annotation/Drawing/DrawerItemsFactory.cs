using System;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class DrawerItemsFactory
{
	public DrawerItemsFactory(BoundingEditor boundingEditor)
	{
		_boundingEditor = boundingEditor;
	}

	public BoundedItemViewModel CreateItemViewModel(BoundedItem item) => item switch
	{
		DetectorItem detectorItem => new DetectorItemViewModel(detectorItem, _boundingEditor),
		Poser2DItem poser2DItem => new Poser2DItemViewModel(poser2DItem, _boundingEditor),
		Poser3DItem poser3DItem => new Poser3DItemViewModel(poser3DItem, _boundingEditor),
		_ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
	};

	private readonly BoundingEditor _boundingEditor;
}