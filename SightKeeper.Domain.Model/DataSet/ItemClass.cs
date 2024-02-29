using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets.Detector;

namespace SightKeeper.Domain.Model.DataSet;

public sealed class ItemClass : ObservableObject
{
	public Id Id { get; private set; }
	public DataSet DataSet { get; private set; }

	public string Name
	{
		get => _name;
		set => SetProperty(ref _name, value);
	}

	public uint Color
	{
		get => _color;
		set => SetProperty(ref _color, value);
	}

	public IReadOnlyCollection<DetectorItem> Items => _items;

	internal ItemClass(DataSet dataSet, string name, uint color)
	{
		DataSet = dataSet;
		_name = name;
		_color = color;
		_items = new ObservableCollection<DetectorItem>();
	}

	internal void AddItem(DetectorItem item) => _items.Add(item);
	internal void RemoveItem(DetectorItem item) => Guard.IsTrue(_items.Remove(item));

	public override string ToString() => Name;

	private readonly ObservableCollection<DetectorItem> _items;
	private string _name;
	private uint _color;

	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private ItemClass()
	{
		DataSet = null!;
		_name = null!;
		_items = null!;
	}
}