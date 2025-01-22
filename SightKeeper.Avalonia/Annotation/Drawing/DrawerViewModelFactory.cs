using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class DrawerViewModelFactory
{
	public DrawerViewModelFactory(Composition composition)
	{
		_composition = composition;
	}

	public DrawerViewModel? CreateDrawerViewModel(DataSet? dataSet)
	{
		switch (dataSet)
		{
			case null:
			case ClassifierDataSet:
				return null;
			case DetectorDataSet detectorDataSet:
				var detectorDrawer = _composition.DetectorDrawerViewModel;
				detectorDrawer.AssetsLibrary = detectorDataSet.AssetsLibrary;
				return detectorDrawer;
			case Poser2DDataSet poser2DDataSet:
				var poser2DDrawer = _composition.Poser2DDrawerViewModel;
				return poser2DDrawer;
			case Poser3DDataSet poser3DDataSet:
				var poser3DDrawer = _composition.Poser3DDrawerViewModel;
				poser3DDrawer.AssetsLibrary = poser3DDataSet.AssetsLibrary;
				return poser3DDrawer;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null);
		}
	}

	private readonly Composition _composition;
}