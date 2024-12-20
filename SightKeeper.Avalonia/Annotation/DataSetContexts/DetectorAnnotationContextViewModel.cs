using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class DetectorAnnotationContextViewModel : DataSetAnnotationContextViewModel, IDisposable
{
	public DetectorDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			ToolBar.Tags = value?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;
			Drawer.AssetsLibrary = value?.AssetsLibrary;
		}
	}

	public override DetectorToolBarViewModel ToolBar { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorAnnotationContextViewModel(
		DetectorToolBarViewModel toolBar,
		DetectorDrawerViewModel drawer,
		ScreenshotsViewModel screenshotsViewModel)
	{
		ToolBar = toolBar;
		Drawer = drawer;
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