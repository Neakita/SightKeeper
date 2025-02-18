using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public sealed class DrawerItemsViewModel
{
	public Screenshot? Screenshot
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

	public IReadOnlyCollection<BoundedItemViewModel> Items { get; }

	public DrawerItemsViewModel(DrawerItemsFactory drawerItemsFactory, ObservableBoundingAnnotator observableBoundingAnnotator)
	{
		_itemsSource.Connect()
			.Transform(drawerItemsFactory.CreateItemViewModel)
			.Bind(out var items)
			.Subscribe();
		Items = items;
		observableBoundingAnnotator.ItemCreated
			.Where(data => data.asset == Asset)
			.Select(data => data.item)
			.Subscribe(_itemsSource.Add);
	}

	private readonly SourceList<BoundedItem> _itemsSource = new();
	private ItemsContainer? Asset => Screenshot == null ? null : AssetsLibrary?.GetOptionalAsset(Screenshot);

	private void UpdateItems()
	{
		_itemsSource.Edit(source =>
		{
			source.Clear();
			if (Asset != null)
				source.AddRange(Asset.Items);
		});
	}
}