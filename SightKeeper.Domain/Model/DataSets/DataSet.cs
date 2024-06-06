using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TTag, TAsset, TAssetsLibrary> where TTag : Tag where TAsset : Asset where TAssetsLibrary : AssetsLibrary<TAsset>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public IReadOnlySet<TTag> Tags => _tags;
	public ScreenshotsLibrary Screenshots { get; }
	public TAssetsLibrary Assets { get; }
	public WeightsLibrary Weights { get; }

	public virtual void DeleteTag(TTag tag)
	{
		bool isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	public override string ToString() => Name;

	protected DataSet(TAssetsLibrary assetsLibrary, string name, ushort resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary();
		Weights = new WeightsLibrary();
		Assets = assetsLibrary;
	}

	protected void AddTag(TTag tag)
	{
		bool isAdded = _tags.Add(tag);
		Guard.IsTrue(isAdded);
	}

	private readonly SortedSet<TTag> _tags = new(TagNameComparer.Instance);
}