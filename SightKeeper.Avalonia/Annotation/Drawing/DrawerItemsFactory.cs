using System;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Domain.DataSets.Assets;
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

	public DrawerItemViewModel CreateItemViewModel(BoundedItem item) => item switch
	{
		DetectorItem detectorItem => new DetectorItemViewModel(detectorItem, _boundingEditor),
		Poser2DItem poser2DItem => throw new NotImplementedException(),
		Poser3DItem poser3DItem => throw new NotImplementedException(),
		_ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
	};

	private readonly BoundingEditor _boundingEditor;
}