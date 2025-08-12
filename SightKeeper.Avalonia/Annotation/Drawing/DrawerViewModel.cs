using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class DrawerViewModel : ViewModel, DrawerDataContext, SelectedItemProvider, IDisposable
{
	[ObservableProperty] public partial ImageDataContext? Image { get; private set; }

	public IReadOnlyCollection<DrawerItemDataContext> Items => _itemsViewModel.Items;
	[ObservableProperty] public partial BoundedItemDataContext? SelectedItem { get; set; }
	public IObservable<AssetItem?> SelectedItemChanged => _selectedItemChanged.AsObservable();
	public BoundingDrawerDataContext BoundingDrawer => _boundingDrawer;
	public KeyPointDrawerDataContext KeyPointDrawer => _keyPointDrawer;

	public DrawerViewModel(
		BoundingDrawerViewModel boundingDrawer,
		AssetItemsViewModel itemsViewModel,
		KeyPointDrawerViewModel keyPointDrawer,
		ImageLoader imageLoader,
		ImageSelection imageSelection)
	{
		_boundingDrawer = boundingDrawer;
		_itemsViewModel = itemsViewModel;
		_keyPointDrawer = keyPointDrawer;
		_imageLoader = imageLoader;
		imageSelection.SelectedImageChanged.Subscribe(HandleImageSelectionChange).DisposeWith(_constructorDisposable);
		_itemsViewModel.PropertyChanged += OnItemsViewModelPropertyChanged;
	}

	public void Dispose()
	{
		_selectedItemChanged.Dispose();
		_constructorDisposable.Dispose();
		_itemsViewModel.PropertyChanged -= OnItemsViewModelPropertyChanged;
	}

	private readonly BoundingDrawerViewModel _boundingDrawer;
	private readonly AssetItemsViewModel _itemsViewModel;
	private readonly KeyPointDrawerViewModel _keyPointDrawer;
	private readonly ImageLoader _imageLoader;
	private readonly Subject<AssetItem?> _selectedItemChanged = new();
	private readonly CompositeDisposable _constructorDisposable = new();

	private void HandleImageSelectionChange(Image? image)
	{
		if (image == null)
		{
			Image = null;
			return;
		}
		Image = new ImageViewModel(_imageLoader, image);
	}

	partial void OnSelectedItemChanged(BoundedItemDataContext? value)
	{
		var assetItem = (value as BoundedItemViewModel)?.Value as AssetItem;
		_selectedItemChanged.OnNext(assetItem);
		_keyPointDrawer.Item = value as PoserItemViewModel;
	}

	private void OnItemsViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(AssetItemsViewModel.Items))
			OnPropertyChanged(e);
	}
}