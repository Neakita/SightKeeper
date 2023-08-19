using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class DataSet<TAsset> where TAsset : Asset
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }

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
		if (ScreenshotsLibrary.HasAnyScreenshots)
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
		ItemClass newItemClass = new(name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	private readonly List<ItemClass> _itemClasses;

	#endregion

	#region Assets

	public IReadOnlyCollection<TAsset> Assets => _assets;

	public TAsset MakeAsset(Screenshot screenshot)
	{
		if (screenshot.Asset != null)
			ThrowHelper.ThrowArgumentException("Asset with same screenshot already exists");
		var asset = Asset.Create<TAsset>(screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(TAsset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
		asset.Screenshot.Asset = null;
	}

	private readonly List<TAsset> _assets;

	#endregion

	public ScreenshotsLibrary ScreenshotsLibrary { get; private set; }
	public WeightsLibrary<TAsset> WeightsLibrary { get; private set; }

	public bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_assets.Any(asset => asset.IsUsesItemClass(itemClass)))
			message = $"Cannot delete item class {itemClass} because he is used in some asset";
		return message == null;
	}
	
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
		WeightsLibrary = new WeightsLibrary<TAsset>();
		ScreenshotsLibrary = new ScreenshotsLibrary();
		_assets = new List<TAsset>();
	}

	private DataSet()
	{
		Name = null!;
		Description = null!;
		_resolution = null!;
		_itemClasses = null!;
		WeightsLibrary = null!;
		ScreenshotsLibrary = null!;
		_assets = null!;
	}
}