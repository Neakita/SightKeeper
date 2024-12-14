using System;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class ClassifierAnnotationContextViewModel : DataSetAnnotationContextViewModel, IDisposable
{
	public override ClassifierToolBarViewModel ToolBar { get; }
	public override DrawerViewModel? Drawer => null;

	public ClassifierAnnotationContextViewModel(ClassifierDataSet dataSet, ClassifierAnnotator annotator, ScreenshotsViewModel screenshotsViewModel)
	{
		ToolBar = new ClassifierToolBarViewModel(dataSet, annotator, screenshotsViewModel);
	}

	public void Dispose()
	{
		ToolBar.Dispose();
	}
}