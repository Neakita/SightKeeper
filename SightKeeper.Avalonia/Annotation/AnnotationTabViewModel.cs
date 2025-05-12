using System;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, IDisposable
{
	public ImagesDataContext Images => _images;
	public DrawerDataContext Drawer => _drawer;
	public AnnotationSideBarComponent SideBar { get; }

	public AnnotationTabViewModel(
		ImagesViewModel images,
		DrawerViewModel drawer,
		AnnotationSideBarComponent sideBar)
	{
		SideBar = sideBar;
		_drawer = drawer;
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
	private readonly DrawerViewModel _drawer;
	private readonly CompositeDisposable _disposable = new();
	private CompositeDisposable? _additionalToolingDisposable;

	private void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_images.Set = value?.Value;
	}

	private void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_drawer.AssetsLibrary = value?.Value.AssetsLibrary as AssetsOwner<ItemsOwner<AssetItem>>;
	}

	private void OnSelectedImageChanged(Image? image)
	{
		_drawer.Image = image;
	}

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