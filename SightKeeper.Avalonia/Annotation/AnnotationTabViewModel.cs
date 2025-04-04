using System;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, IDisposable
{
	public ImagesDataContext Images => _images;
	public AnnotationDrawerComponent Drawer { get; }
	public AnnotationSideBarComponent SideBar { get; }

	public AnnotationTabViewModel(
		ImagesViewModel images,
		AnnotationDrawerComponent drawer,
		AnnotationSideBarComponent sideBar)
	{
		SideBar = sideBar;
		Drawer = drawer;
		_images = images;
		SideBar.SelectedImageSetChanged
			.Subscribe(OnSelectedImageSetChanged)
			.DisposeWith(_disposable);
		SideBar.SelectedDataSetChanged
			.Subscribe(OnSelectedDataSetChanged)
			.DisposeWith(_disposable);
		_images.SelectedImageChanged
			.Subscribe(OnSelectedImageChanged)
			.DisposeWith(_disposable);
		SideBar.AdditionalToolingChanged
			.Subscribe(OnAdditionalToolingChanged)
			.DisposeWith(_disposable);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly ImagesViewModel _images;
	private readonly CompositeDisposable _disposable = new();
	private CompositeDisposable? _additionalToolingDisposable;

	private void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_images.Set = value?.Value;
	}

	private void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		Drawer.AssetsLibrary = value?.Value.AssetsLibrary as AssetsOwner<ItemsOwner>;
	}

	private void OnSelectedImageChanged(Image image)
	{
		Drawer.Image = image;
	}

	private void OnAdditionalToolingChanged(object? value)
	{
		_additionalToolingDisposable?.Dispose();
		_additionalToolingDisposable = new CompositeDisposable();
		if (value is TagSelection<Tag> selection)
		{
			SetDrawerTag(selection.SelectedTag);
		}
		if (value is ObservableTagSelection<Tag> observableSelection)
		{
			observableSelection.SelectedTagChanged.Subscribe(SetDrawerTag).DisposeWith(_additionalToolingDisposable);
		}
		if (value is SelectedItemConsumer selectedItemConsumer)
		{
			selectedItemConsumer.SelectedItem = Drawer.SelectedItem;
			Drawer.SelectedItemChanged.Subscribe(item => selectedItemConsumer.SelectedItem = item).DisposeWith(_additionalToolingDisposable);
		}
	}

	private void SetDrawerTag(Tag? tag)
	{
		Drawer.Tag = tag;
	}
}