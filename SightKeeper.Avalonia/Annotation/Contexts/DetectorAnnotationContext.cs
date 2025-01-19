using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using TagSelectionViewModel = SightKeeper.Avalonia.Annotation.SideBars.TagSelectionViewModel;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class DetectorAnnotationContext : DataSetAnnotationContext, IDisposable
{
	public DetectorDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			SideBar.Tags = value?.TagsLibrary.Tags ?? ReadOnlyCollection<Tag>.Empty;
			Drawer.AssetsLibrary = value?.AssetsLibrary;
		}
	}

	public override TagSelectionViewModel SideBar { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorAnnotationContext(
		TagSelectionViewModel sideBar,
		DetectorDrawerViewModel drawer,
		ScreenshotsViewModel screenshotsViewModel)
	{
		SideBar = sideBar;
		Drawer = drawer;
		SideBar.PropertyChanged += OnSideBarPropertyChanged;
		_disposable = screenshotsViewModel.SelectedScreenshotChanged.Subscribe(OnScreenshotChanged);
		Drawer.Screenshot = screenshotsViewModel.SelectedScreenshot?.Value;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;

	private void OnSideBarPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(SideBar.SelectedTag))
			Drawer.SetTag(SideBar.SelectedTag);
	}

	private void OnScreenshotChanged(ScreenshotViewModel? screenshot)
	{
		Drawer.Screenshot = screenshot?.Value;
	}
}