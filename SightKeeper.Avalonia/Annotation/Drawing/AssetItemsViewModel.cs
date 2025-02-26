using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class AssetItemsViewModel
{
	public Image? Screenshot
	{
		get;
		set
		{
			field = value;
			UpdateItems();
		}
	}

	public AssetsContainer<ItemsContainer>? AssetsLibrary
	{
		get;
		set
		{
			field = value;
			UpdateItems();
		}
	}

	public IReadOnlyCollection<DrawerItemDataContext> Items => _items;

	public AssetItemsViewModel(
		DrawerItemsFactory drawerItemsFactory,
		KeyPointViewModelFactory keyPointFactory,
		ObservableBoundingAnnotator observableBoundingAnnotator,
		ObservablePoserAnnotator observablePoserAnnotator)
	{
		_drawerItemsFactory = drawerItemsFactory;
		_keyPointFactory = keyPointFactory;
		observableBoundingAnnotator.ItemCreated
			.Where(data => data.asset == Asset)
			.Select(data => data.item)
			.Subscribe(OnItemCreated)
			.DisposeWith(_disposable);
		observablePoserAnnotator.KeyPointCreated.Subscribe(OnKeyPointCreated).DisposeWith(_disposable);
		observablePoserAnnotator.KeyPointDeleted.Subscribe(OnKeyPointDeleted).DisposeWith(_disposable);
	}

	private readonly DrawerItemsFactory _drawerItemsFactory;
	private readonly KeyPointViewModelFactory _keyPointFactory;
	private readonly AvaloniaList<DrawerItemDataContext> _items = new();
	private readonly CompositeDisposable _disposable = new();
	private ItemsContainer? Asset => Screenshot == null ? null : AssetsLibrary?.GetOptionalAsset(Screenshot);

	private void UpdateItems()
	{
		_items.Clear();
		if (Asset == null)
			return;
		var items = Asset.Items
			.Select(_drawerItemsFactory.CreateItemViewModel)
			.SelectMany(AddKeyPoints);
		_items.Add(items);
	}

	private IEnumerable<DrawerItemDataContext> AddKeyPoints(BoundedItemDataContext item)
	{
		yield return item;
		if (item is not PoserItemViewModel poserItem)
			yield break;
		foreach (var keyPoint in poserItem.Value.KeyPoints)
		{
			var keyPointViewModel = _keyPointFactory.CreateViewModel(poserItem, keyPoint);
			yield return keyPointViewModel;
		}
	}

	private void OnItemCreated(BoundedItem item)
	{
		var itemViewModel = _drawerItemsFactory.CreateItemViewModel(item);
		_items.Add(itemViewModel);
	}

	private void OnKeyPointCreated((PoserItem item, KeyPoint keyPoint) tuple)
	{
		var itemViewModel = _items.OfType<PoserItemViewModel>().Single(viewModel => viewModel.Value == tuple.item);
		var keyPointViewModel = _keyPointFactory.CreateViewModel(itemViewModel, tuple.keyPoint);
		_items.Add(keyPointViewModel);
	}

	private void OnKeyPointDeleted((PoserItem item, KeyPoint keyPoint) tuple)
	{
		var itemViewModel = _items.OfType<PoserItemViewModel>().Single(viewModel => viewModel.Value == tuple.item);
		var keyPointViewModel = itemViewModel.KeyPoints.Single(viewModel => viewModel.Value == tuple.keyPoint);
		itemViewModel.RemoveKeyPoint(keyPointViewModel);
		var isRemoved = _items.Remove(keyPointViewModel);
		Guard.IsTrue(isRemoved);
	}
}