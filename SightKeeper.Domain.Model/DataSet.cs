using System.Collections.ObjectModel;

namespace SightKeeper.Domain.Model;

public sealed class DataSet : Entity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }

	public Library Screenshots { get; }
	public WeightsLibrary Weights { get; }
	public IReadOnlyCollection<Asset> Assets => _assets;
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	
	public ItemClass CreateItemClass(string name, uint color)
	{
		ItemClass newItemClass = new(name, color);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public bool DeleteItemClass(ItemClass itemClass)
	{
		if (_assets.SelectMany(asset => asset.Items).Any(item => item.ItemClass == itemClass))
			throw new InvalidOperationException($"Item class {itemClass} is in use");
		return _itemClasses.Remove(itemClass);
	}
	
	public override string ToString() => Name;

	public DataSet(string name, ushort resolution = 320)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new Library();
		Weights = new WeightsLibrary();
		_assets = new ObservableCollection<Asset>();
	}

	public Asset MakeAsset(Screenshot screenshot)
	{
		var asset = new Asset();
		screenshot.Asset = asset;
		_assets.Add(asset);
		return asset;
	}

	public bool DeleteAsset(Asset asset)
	{
		return _assets.Remove(asset);
	}

	private readonly SortedSet<ItemClass> _itemClasses = new(ItemClassNameComparer.Instance);
	private readonly ObservableCollection<Asset> _assets;
}