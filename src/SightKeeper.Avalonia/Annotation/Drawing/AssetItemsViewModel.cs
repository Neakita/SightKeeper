using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling.DataSet;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class AssetItemsViewModel : ViewModel, IDisposable
{
	[ObservableProperty]
	public partial IReadOnlyCollection<DrawerItemDataContext> Items { get; private set; } = ReadOnlyCollection<DrawerItemDataContext>.Empty;

	public AssetItemsViewModel(DrawerItemsFactory drawerItemsFactory, DataSetSelection dataSetSelection, ImageSelection imageSelection)
	{
		_drawerItemsFactory = drawerItemsFactory;
		dataSetSelection.SelectedDataSetChanged
			.Subscribe(HandleDataSetSelectionChange)
			.DisposeWith(_constructorDisposable);
		imageSelection.SelectedImageChanged
			.Subscribe(HandleImageSelectionChange)
			.DisposeWith(_constructorDisposable);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly DrawerItemsFactory _drawerItemsFactory;
	private readonly CompositeDisposable _constructorDisposable = new();
	private AssetsContainer<ItemsContainer<DetectorItem>>? _assetsLibrary;
	private ManagedImage? _image;
	private ItemsContainer<DetectorItem>? Asset => _image == null ? null : _assetsLibrary?.GetOptionalAsset(_image);
	private IDisposable _assetImagesSubscription = Disposable.Empty;
	private IDisposable _itemsDisposable = Disposable.Empty;

	private void UpdateItems()
	{
		_itemsDisposable.Dispose();
		_itemsDisposable = Disposable.Empty;
		if (Asset == null)
		{
			Items = ReadOnlyCollection<DrawerItemDataContext>.Empty;
			return;
		}

		var disposable = new CompositeDisposable();
		var items = (ReadOnlyObservableList<DetectorItem>)Asset.Items;

		ReadOnlyObservableList<DrawerItemDataContext> observableItems = items
			.Transform(_drawerItemsFactory.CreateItemViewModel)
			.DisposeMany()
			.ToObservableList();
		observableItems.DisposeWith(disposable);
		if (items is ReadOnlyObservableList<PoserItem>)
		{
			var observableKeyPoints = observableItems.TransformMany(item =>
			{
				var poserItem = (PoserItemViewModel)item;
				return poserItem.KeyPoints;
			});
			observableItems = observableItems.Concatenate(observableKeyPoints).ToObservableList();
			observableItems.DisposeWith(disposable);
		}
		Items = observableItems
			.ToObservableList()
			.ToReadOnlyNotifyingList();
		_itemsDisposable = disposable;
	}

	private void HandleDataSetSelectionChange(DataSet<Tag, Asset>? set)
	{
		_assetImagesSubscription.Dispose();
		_assetImagesSubscription = Disposable.Empty;
		_assetsLibrary = set?.AssetsLibrary as AssetsContainer<ItemsContainer<DetectorItem>>;
		if (_assetsLibrary != null)
		{
			var observableAssetImages = (Vibrance.ReadOnlyObservableCollection<ManagedImage>)_assetsLibrary.Images;
			_assetImagesSubscription = observableAssetImages.Subscribe(_ => UpdateItems());
		}
		UpdateItems();
	}

	private void HandleImageSelectionChange(ManagedImage? image)
	{
		_image = image;
		UpdateItems();
	}
}