using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed partial class DrawerViewModel : ViewModel, AnnotationDrawerComponent
{
	[ObservableProperty] public partial Image? Screenshot { get; set; }

	public Tag? Tag
	{
		get;
		set
		{
			field = value;
			_boundingDrawer.Tag = null;
			_keyPointDrawer.Tag = null;
			if (value?.Owner is PoserTag)
				_keyPointDrawer.Tag = value;
			else
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
	public KeyPointDrawerDataContext KeyPointDrawer => _keyPointDrawer;

	public DrawerViewModel(BoundingDrawerViewModel boundingDrawer, AssetItemsViewModel itemsViewModel, KeyPointDrawerViewModel keyPointDrawer)
	{
		_boundingDrawer = boundingDrawer;
		_itemsViewModel = itemsViewModel;
		_keyPointDrawer = keyPointDrawer;
	}

	private readonly BoundingDrawerViewModel _boundingDrawer;
	private readonly AssetItemsViewModel _itemsViewModel;
	private readonly KeyPointDrawerViewModel _keyPointDrawer;
	private readonly Subject<BoundedItemDataContext?> _selectedItemChanged = new();

	partial void OnScreenshotChanged(Image? value)
	{
		_boundingDrawer.Screenshot = value;
		_itemsViewModel.Screenshot = value;
	}

	partial void OnSelectedItemChanged(BoundedItemDataContext? value)
	{
		_selectedItemChanged.OnNext(value);
		_keyPointDrawer.Item = value as PoserItemViewModel;
	}
}