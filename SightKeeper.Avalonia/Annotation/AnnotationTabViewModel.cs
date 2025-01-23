using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, IDisposable
{
	public ScreenshotsViewModel Screenshots { get; }
	public DrawerViewModel Drawer { get; }
	public SideBarViewModel SideBar { get; }

	public void Dispose()
	{
		_disposable.Dispose();
	}

	internal AnnotationTabViewModel(
		ScreenshotsViewModel screenshots,
		DrawerViewModel drawer,
		SideBarViewModel sideBar)
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

	private void OnAdditionalToolingChanged(ViewModel? value)
	{
		_tagSelectionDisposable?.Dispose();
		_tagSelectionDisposable = null;
		if (value is not TagSelectionViewModel tagSelection)
			return;
		tagSelection.PropertyChanged += OnTagSelectionPropertyChanged;
		_tagSelectionDisposable = Disposable.Create(() => tagSelection.PropertyChanged -= OnTagSelectionPropertyChanged);
		return;
		void OnTagSelectionPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			Guard.IsNotNull(sender);
			var tagSelectionViewModel = (TagSelectionViewModel)sender;
			if (args.PropertyName == nameof(tagSelection.SelectedTag))
				Drawer.Tag = tagSelectionViewModel.SelectedTag;
		}
	}
}