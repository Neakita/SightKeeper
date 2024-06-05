﻿using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public IReadOnlySet<ItemClass> ItemClasses => _itemClasses;
	public ScreenshotsLibrary Screenshots { get; }
	public AssetsLibrary Assets { get; }
	public WeightsLibrary Weights { get; }

	public DataSet(string name, ushort resolution = 320)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary();
		Weights = new WeightsLibrary();
		Assets = new AssetsLibrary();
	}
	public ItemClass CreateItemClass(string name, uint color)
	{
		ItemClass newItemClass = new(name, color);
		bool isAdded = _itemClasses.Add(newItemClass);
		Guard.IsTrue(isAdded);
		return newItemClass;
	}
	public void DeleteItemClass(ItemClass itemClass)
	{
		// TODO
		/*if (Assets.SelectMany(asset => asset.Items).Any(item => item.ItemClass == itemClass))
			throw new InvalidOperationException($"Item class \"{itemClass}\" is in use");*/
		bool isRemoved = _itemClasses.Remove(itemClass);
		Guard.IsTrue(isRemoved);
	}
	public override string ToString() => Name;

	private readonly SortedSet<ItemClass> _itemClasses = new(ItemClassNameComparer.Instance);
}