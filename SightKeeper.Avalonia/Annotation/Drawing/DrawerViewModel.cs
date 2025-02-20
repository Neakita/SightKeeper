using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class DrawerViewModel : ViewModel, AnnotationDrawerComponent
{
	[ObservableProperty] public partial Screenshot? Screenshot { get; set; }

	public Tag? Tag
	{
		get;
		set
		{
			field = value;
			_boundingDrawer.Tag = value;
		}
	}

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

	public IReadOnlyCollection<DrawerItemDataContext> Items => _itemsViewModel.Items;
	[ObservableProperty] public partial BoundedItemDataContext? SelectedItem { get; set; }
	public IObservable<BoundedItemDataContext?> SelectedItemChanged => _selectedItemChanged.AsObservable();
	public BoundingDrawerDataContext BoundingDrawer => _boundingDrawer;

	public DrawerViewModel(BoundingDrawerViewModel boundingDrawer, AssetItemsViewModel itemsViewModel)
	{
		_boundingDrawer = boundingDrawer;
		_itemsViewModel = itemsViewModel;
	}

	private readonly BoundingDrawerViewModel _boundingDrawer;
	private readonly AssetItemsViewModel _itemsViewModel;
	private readonly Subject<BoundedItemDataContext?> _selectedItemChanged = new();

	partial void OnScreenshotChanged(Screenshot? value)
	{
		_boundingDrawer.Screenshot = value;
		_itemsViewModel.Screenshot = value;
	}

	partial void OnSelectedItemChanged(BoundedItemDataContext? value)
	{
		_selectedItemChanged.OnNext(value);
	}
}