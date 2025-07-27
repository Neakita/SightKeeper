using System;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, AnnotationTabDataContext, IDisposable
{
	public ImagesDataContext Images => _images;
	public DrawerDataContext Drawer => _drawer;
	public SideBarDataContext SideBar { get; }

	public AnnotationTabViewModel(
		ImagesViewModel images,
		DrawerViewModel drawer,
		AnnotationSideBarComponent sideBar)
	{
		SideBar = sideBar;
		_drawer = drawer;
		_images = images;
		sideBar.AdditionalToolingChanged
			.Subscribe(OnAdditionalToolingChanged)
			.DisposeWith(_disposable);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly ImagesViewModel _images;
	private readonly DrawerViewModel _drawer;
	private readonly CompositeDisposable _disposable = new();
	private CompositeDisposable? _additionalToolingDisposable;

	private void OnAdditionalToolingChanged(object? value)
	{
		_additionalToolingDisposable?.Dispose();
		_additionalToolingDisposable = new CompositeDisposable();
		if (value is TagSelection tagSelection)
		{
			SetDrawerTag(tagSelection.SelectedTag);
		}
		if (value is ObservableTagSelection observableTagSelection)
		{
			observableTagSelection.SelectedTagChanged.Subscribe(SetDrawerTag).DisposeWith(_additionalToolingDisposable);
		}
		if (value is SelectedItemConsumer selectedItemConsumer)
		{
			selectedItemConsumer.SelectedItem = Drawer.SelectedItem;
			_drawer.SelectedItemChanged.Subscribe(item => selectedItemConsumer.SelectedItem = item).DisposeWith(_additionalToolingDisposable);
		}
	}

	private void SetDrawerTag(Tag? tag)
	{
		_drawer.Tag = tag;
	}
}