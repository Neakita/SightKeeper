﻿using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public DataSet DataSet { get; private set; }
	public string Name { get; set; }
	public uint Color { get; set; }
	public IReadOnlyCollection<DetectorItem> Items => _items;

	internal ItemClass(DataSet dataSet, string name, uint color)
	{
		DataSet = dataSet;
		Name = name;
		Color = color;
		_items = new List<DetectorItem>();
	}

	internal void AddItem(DetectorItem item) => _items.Add(item);
	internal void RemoveItem(DetectorItem item) => Guard.IsTrue(_items.Remove(item));

	public override string ToString() => Name;

	private readonly List<DetectorItem> _items;

	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private ItemClass()
	{
		DataSet = null!;
		Name = null!;
		_items = null!;
	}
}