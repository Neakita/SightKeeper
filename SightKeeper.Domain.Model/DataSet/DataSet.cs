using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public abstract class DataSet
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

	public abstract bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message);
	
	private readonly List<ItemClass> _itemClasses;

	#endregion
	
	public override string ToString() => Name;

	protected DataSet(string name) : this(name, new Resolution())
	{
	}

	protected DataSet(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		ScreenshotsLibrary = new ScreenshotsLibrary(this);
		WeightsLibrary = new WeightsLibrary();
	}

	protected DataSet()
	{
		Name = null!;
		Description = null!;
		_resolution = null!;
		_itemClasses = null!;
		ScreenshotsLibrary = null!;
	}
}

public sealed class DataSet<TAsset> : DataSet where TAsset : Asset
{
	#region Assets

	public IReadOnlyCollection<TAsset> Assets => _assets;

	public TAsset MakeAsset(Screenshot screenshot)
	{
		var asset = Asset.Create(this, screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(TAsset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
	}

	private readonly List<TAsset> _assets;

	#endregion

	public override bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_assets.Any(asset => asset.IsUsesItemClass(itemClass)))
			message = $"Cannot delete item class {itemClass} because he is used in some asset";
		return message == null;
	}

	public DataSet(string name) : this(name, new Resolution())
	{
	}

	public DataSet(string name, Resolution resolution) : base(name, resolution)
	{
		_assets = new List<TAsset>();
	}

	private DataSet()
	{
		_assets = null!;
	}
}