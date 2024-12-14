using System;
using System.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class DetectorAnnotationContextViewModel : DataSetAnnotationContextViewModel, IDisposable
{
	public override DetectorToolBarViewModel ToolBar { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorAnnotationContextViewModel(
		DetectorDataSet dataSet,
		DetectorAnnotator detectorAnnotator,
		ScreenshotsViewModel screenshotsViewModel)
	{
		ToolBar = new DetectorToolBarViewModel(dataSet.TagsLibrary.Tags);
		Drawer = new DetectorDrawerViewModel(detectorAnnotator, dataSet);
		ToolBar.PropertyChanged += OnToolBarPropertyChanged;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(OnScreenshotChanged);
		Drawer.Screenshot = screenshotsViewModel.SelectedScreenshot;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;

	private void OnToolBarPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ToolBar.Tag))
			Drawer.SetTag(ToolBar.Tag);
	}

	private void OnScreenshotChanged(ScreenshotViewModel? screenshot)
	{
		Drawer.Screenshot = screenshot;
	}
}