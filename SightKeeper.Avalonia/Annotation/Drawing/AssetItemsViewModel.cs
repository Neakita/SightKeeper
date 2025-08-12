using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
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
		dataSetSelection.SelectedDataSetChanged.Subscribe(HandleDataSetSelectionChange).DisposeWith(_constructorDisposable);
		imageSelection.SelectedImageChanged.Subscribe(HandleImageSelectionChange).DisposeWith(_constructorDisposable);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly DrawerItemsFactory _drawerItemsFactory;
	private readonly CompositeDisposable _constructorDisposable = new();
	private AssetsContainer<ItemsContainer<AssetItem>>? _assetsLibrary;
	private Image? _image;
	private ItemsContainer<AssetItem>? Asset => _image == null ? null : _assetsLibrary?.GetOptionalAsset(_image);

	private void UpdateItems()
	{
		if (Asset == null)
		{
			Items = ReadOnlyCollection<DrawerItemDataContext>.Empty;
			return;
		}
		var items = (ReadOnlyObservableList<AssetItem>)Asset.Items;
		Items = items.Transform(_drawerItemsFactory.CreateItemViewModel).ToObservableList().ToReadOnlyNotifyingList();
	}

	private void HandleDataSetSelectionChange(DataSet? set)
	{
		_assetsLibrary = set?.AssetsLibrary as AssetsContainer<ItemsContainer<AssetItem>>;
		UpdateItems();
	}

	private void HandleImageSelectionChange(Image? image)
	{
		_image = image;
		UpdateItems();
	}
}