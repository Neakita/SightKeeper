﻿using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DTag : PoserTag
{
	public IReadOnlyList<KeyPointTag2D> KeyPoints => _keyPoints.AsReadOnly();
	public IReadOnlyList<NumericItemProperty> Properties => _properties.AsReadOnly();
	public override IReadOnlyCollection<Poser2DItem> Items => _items;
	public Poser2DTagsLibrary Library { get; }
	public Poser2DDataSet DataSet => Library.DataSet;

	public KeyPointTag2D CreateKeyPoint(string name)
	{
		Guard.IsEmpty(Items);
		KeyPointTag2D tag = new(name, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag2D tag)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_keyPoints.Remove(tag));
	}

	public NumericItemProperty CreateProperty(string name, double minimumValue, double maximumValue)
	{
		NumericItemProperty property = new(this, name, minimumValue, maximumValue);
		_properties.Add(property);
		return property;
	}

	public void RemoveProperty(NumericItemProperty property)
	{
		_properties.Remove(property);
	}

	internal Poser2DTag(string name, Poser2DTagsLibrary library) : base(name, library)
	{
		Library = library;
		_keyPoints = new List<KeyPointTag2D>();
		_properties = new List<NumericItemProperty>();
		_items = new HashSet<Poser2DItem>();
	}

	internal void AddItem(Poser2DItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(Poser2DItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	protected override IEnumerable<Tag> Siblings => Library;

	private readonly List<KeyPointTag2D> _keyPoints;
	private readonly List<NumericItemProperty> _properties;
	private readonly HashSet<Poser2DItem> _items;
}