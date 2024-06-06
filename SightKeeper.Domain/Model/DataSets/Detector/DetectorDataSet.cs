using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public IReadOnlySet<Tag> Tags => _tags;
	public ScreenshotsLibrary Screenshots { get; }
	public DetectorAssetsLibrary DetectorAssets { get; }
	public WeightsLibrary Weights { get; }

	public DetectorDataSet(string name, ushort resolution = 320)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary();
		Weights = new WeightsLibrary();
		DetectorAssets = new DetectorAssetsLibrary();
	}
	public Tag CreateTag(string name, uint color)
	{
		Tag newTag = new(name, color);
		bool isAdded = _tags.Add(newTag);
		Guard.IsTrue(isAdded);
		return newTag;
	}
	public void DeleteTag(Tag tag)
	{
		// TODO
		/*if (Assets.SelectMany(asset => asset.Items).Any(item => item.Tag == tag))
			throw new InvalidOperationException($"Item class \"{tag}\" is in use");*/
		bool isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}
	public override string ToString() => Name;

	private readonly SortedSet<Tag> _tags = new(TagNameComparer.Instance);
}