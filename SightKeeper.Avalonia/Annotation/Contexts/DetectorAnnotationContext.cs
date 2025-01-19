using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class DetectorAnnotationContext : DataSetAnnotationContext, IDisposable
{
	public DetectorDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			Annotation.Tags = value?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;
			Drawer.AssetsLibrary = value?.AssetsLibrary;
		}
	}

	public override DetectorToolBarViewModel Annotation { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorAnnotationContext(
		DetectorToolBarViewModel annotation,
		DetectorDrawerViewModel drawer,
		ScreenshotsViewModel screenshotsViewModel)
	{
		Annotation = annotation;
		Drawer = drawer;
		Annotation.PropertyChanged += OnAnnotationPropertyChanged;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(OnScreenshotChanged);
		Drawer.Screenshot = screenshotsViewModel.SelectedScreenshot?.Value;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;

	private void OnAnnotationPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Annotation.SelectedTag))
			Drawer.SetTag(Annotation.SelectedTag);
	}

	private void OnScreenshotChanged(ScreenshotViewModel? screenshot)
	{
		Drawer.Screenshot = screenshot?.Value;
	}
}