using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class DrawerViewModel : ViewModel
{
	[ObservableProperty] public partial Screenshot? Screenshot { get; set; }
	[ObservableProperty] public partial Tag? Tag { get; set; }

	public AssetsOwner<ItemsOwner>? AssetsLibrary
	{
		get;
		set
		{
			field = value;
			_boundingDrawer.AssetsLibrary = value;
			_itemsViewModel.AssetsLibrary = value;
		}
	}

	public IReadOnlyCollection<DrawerItemViewModel> Items => _itemsViewModel.Items;
	[ObservableProperty] public partial DrawerItemViewModel? SelectedItem { get; set; }
	public ICommand CreateItemCommand => _boundingDrawer.CreateItemCommand;

	public DrawerViewModel(BoundingDrawerViewModel boundingDrawer, DrawerItemsViewModel itemsViewModel)
	{
		_boundingDrawer = boundingDrawer;
		_itemsViewModel = itemsViewModel;
	}

	private readonly BoundingDrawerViewModel _boundingDrawer;
	private readonly DrawerItemsViewModel _itemsViewModel;

	partial void OnScreenshotChanged(Screenshot? value)
	{
		_boundingDrawer.Screenshot = value;
		_itemsViewModel.Screenshot = value;
	}

	partial void OnTagChanged(Tag? value)
	{
		_boundingDrawer.Tag = value;
	}
}