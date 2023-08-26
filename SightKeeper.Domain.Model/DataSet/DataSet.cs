using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ScreenshotsLibrary ScreenshotsLibrary { get; private set; }
	public WeightsLibrary WeightsLibrary { get; set; }

	#region Resolution

	public Resolution Resolution
	{
		get => _resolution;
		set
		{
			if (value.Equals(_resolution))
				return;
			if (!CanChangeResolution(out var message))
				ThrowHelper.ThrowInvalidOperationException(message);
			_resolution = value;
		}
	}

	public bool CanChangeResolution([NotNullWhen(false)] out string? message)
	{
		if (ScreenshotsLibrary.Screenshots.Any())
			message = "Can't change resolution when there are screenshots";
		else
			message = null;
		return message == null;
	}

	private Resolution _resolution;

	#endregion

	#region ItemClasses
	
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	
	public ItemClass CreateItemClass(string name)
	{
		if (_itemClasses.Any(itemClass => itemClass.Name == name))
			ThrowHelper.ThrowArgumentException($"Item class with name \"{name}\" already exists");
		ItemClass newItemClass = new(this, name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	public bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_assets.Any(asset => asset.Items.Any(item => item.ItemClass == itemClass)))
			message = $"Cannot delete item class {itemClass} because he is used in some asset";
		return message == null;
	}
	
	private readonly List<ItemClass> _itemClasses;
	private readonly List<Asset> _assets;

	#endregion
	
	public override string ToString() => Name;

	public DataSet(string name) : this(name, new Resolution())
	{
	}

	public DataSet(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		ScreenshotsLibrary = new ScreenshotsLibrary(this);
		WeightsLibrary = new WeightsLibrary(this);
		_assets = new List<Asset>();
	}

	private DataSet()
	{
		Name = null!;
		Description = null!;
		_resolution = null!;
		_itemClasses = null!;
		ScreenshotsLibrary = null!;
		WeightsLibrary = null!;
		_assets = null!;
	}
	
	#region Assets

	public IReadOnlyCollection<Asset> Assets => _assets;

	public Asset MakeAsset(Screenshot screenshot)
	{
		var asset = new Asset(this, screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(Asset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
	}

	#endregion
}