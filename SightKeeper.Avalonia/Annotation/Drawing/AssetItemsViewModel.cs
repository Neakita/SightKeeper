using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class AssetItemsViewModel : ViewModel
{
	public Image? Image
	{
		get;
		set
		{
			field = value;
			OnImageChanged();
		}
	}

	public AssetsContainer<ItemsContainer<AssetItem>>? AssetsLibrary
	{
		get;
		set
		{
			field = value;
			OnImageChanged();
		}
	}

	[ObservableProperty]
	public partial IReadOnlyCollection<DrawerItemDataContext> Items { get; private set; } = ReadOnlyCollection<DrawerItemDataContext>.Empty;

	public AssetItemsViewModel(DrawerItemsFactory drawerItemsFactory)
	{
		_drawerItemsFactory = drawerItemsFactory;
	}

	private readonly DrawerItemsFactory _drawerItemsFactory;
	private ItemsContainer<AssetItem>? Asset => Image == null ? null : AssetsLibrary?.GetOptionalAsset(Image);

	private void OnImageChanged()
	{
		if (Asset == null)
			return;
		var items = (ObservableList<AssetItem>)Asset.Items;
		var itemViewModels = items.Transform(_drawerItemsFactory.CreateItemViewModel).ToObservableList();
		Items = itemViewModels;
	}
}