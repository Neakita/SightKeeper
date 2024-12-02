using System;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel : ViewModel
{
	public static DataSetContextViewModel? Create(
		DataSet? dataSet,
		ObservableDataAccess<Screenshot> observableScreenshotsDataAccess,
		ScreenshotImageLoader imageLoader,
		ClassifierAnnotator classifierAnnotator,
		DetectorAnnotator detectorAnnotator) =>
		dataSet switch
		{
			null => null,
			ClassifierDataSet classifier => new ClassifierContextViewModel(
				classifier,
				observableScreenshotsDataAccess,
				imageLoader,
				classifierAnnotator),
			DetectorDataSet detector => new DetectorContextViewModel(
				detector,
				observableScreenshotsDataAccess,
				imageLoader,
				detectorAnnotator),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null)
		};

	public abstract ScreenshotsViewModel Screenshots { get; }
	public abstract ToolBarViewModel? ToolBar { get; }
	public abstract DrawerViewModel? Drawer { get; }
}