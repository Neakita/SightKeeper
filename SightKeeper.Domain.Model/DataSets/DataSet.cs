namespace SightKeeper.Domain.Model.DataSets;

public sealed class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	public ScreenshotsLibrary Screenshots { get; }
	public AssetsLibrary Assets { get; }
	public WeightsLibrary Weights { get; }

	public DataSet(string name, ushort resolution = 320)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary(this);
		Weights = new WeightsLibrary(this);
		Assets = new AssetsLibrary(this);
	}
	public ItemClass CreateItemClass(string name, uint color)
	{
		ItemClass newItemClass = new(name, color, this);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	public bool DeleteItemClass(ItemClass itemClass)
	{
		if (Screenshots.Where(screenshot => screenshot.Asset != null).Select(screenshot => screenshot.Asset!).SelectMany(asset => asset.Items).Any(item => item.ItemClass == itemClass))
			throw new InvalidOperationException($"Item class {itemClass} is in use");
		return _itemClasses.Remove(itemClass);
	}
	public override string ToString() => Name;

	private readonly SortedSet<ItemClass> _itemClasses = new(ItemClassNameComparer.Instance);
}