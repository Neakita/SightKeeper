using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
	public Image? Image
	{
		get;
		set
		{
			field = value;
			UpdateItems();
		}
	}

	[ObservableProperty]
	public partial IReadOnlyCollection<DrawerItemDataContext> Items { get; private set; } = ReadOnlyCollection<DrawerItemDataContext>.Empty;

	public AssetItemsViewModel(DrawerItemsFactory drawerItemsFactory, DataSetSelection dataSetSelection)
	{
		_drawerItemsFactory = drawerItemsFactory;
		_disposable = dataSetSelection.SelectedDataSetChanged.Subscribe(HandleDataSetSelectionChange);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly DrawerItemsFactory _drawerItemsFactory;
	private readonly IDisposable _disposable;
	private AssetsContainer<ItemsContainer<AssetItem>>? _assetsLibrary;
	private ItemsContainer<AssetItem>? Asset => Image == null ? null : _assetsLibrary?.GetOptionalAsset(Image);

	private void UpdateItems()
	{
		if (Asset == null)
			return;
		var items = (ObservableList<AssetItem>)Asset.Items;
		var itemViewModels = items.Transform(_drawerItemsFactory.CreateItemViewModel).ToObservableList();
		Items = itemViewModels;
	}

	private void HandleDataSetSelectionChange(DataSet? set)
	{
		_assetsLibrary = set?.AssetsLibrary as AssetsContainer<ItemsContainer<AssetItem>>;
		UpdateItems();
	}
}