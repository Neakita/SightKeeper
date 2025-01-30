using System;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, IDisposable
{
	public AnnotationScreenshotsComponent Screenshots { get; }
	public AnnotationDrawerComponent Drawer { get; }
	public AnnotationSideBarComponent SideBar { get; }

	public void Dispose()
	{
		_disposable.Dispose();
	}

	internal AnnotationTabViewModel(
		AnnotationScreenshotsComponent screenshots,
		AnnotationDrawerComponent drawer,
		AnnotationSideBarComponent sideBar)
	{
		SideBar = sideBar;
		Drawer = drawer;
		Screenshots = screenshots;
		SideBar.SelectedScreenshotsLibraryChanged
			.Subscribe(OnSelectedScreenshotsLibraryChanged)
			.DisposeWith(_disposable);
		SideBar.SelectedDataSetChanged
			.Subscribe(OnSelectedDataSetChanged)
			.DisposeWith(_disposable);
		Screenshots.SelectedScreenshotChanged
			.Subscribe(_ => OnSelectedScreenshotChanged())
			.DisposeWith(_disposable);
		SideBar.AdditionalToolingChanged
			.Subscribe(OnAdditionalToolingChanged)
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private IDisposable? _tagSelectionDisposable;

	private void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		Screenshots.Library = value?.Value;
	}

	private void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		Drawer.AssetsLibrary = value?.Value.AssetsLibrary as AssetsOwner<ItemsOwner>;
	}

	private void OnSelectedScreenshotChanged()
	{
		Drawer.Screenshot = Screenshots.SelectedScreenshot;
	}

	private void OnAdditionalToolingChanged(object? value)
	{
		_tagSelectionDisposable?.Dispose();
		_tagSelectionDisposable = null;
		if (value is TagSelection<Tag> selection)
		{
			SetDrawerTag(selection.SelectedTag);
		}
		if (value is ObservableTagSelection<Tag> observableSelection)
		{
			_tagSelectionDisposable = observableSelection.SelectedTagChanged.Subscribe(SetDrawerTag);
		}
	}

	private void SetDrawerTag(Tag? tag)
	{
		Drawer.Tag = tag;
	}
}