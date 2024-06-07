using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TTag> where TTag : Tag
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public IReadOnlyCollection<TTag> Tags => _tags;
	public ScreenshotsLibrary Screenshots { get; }
	public WeightsLibrary Weights { get; }

	public virtual void DeleteTag(TTag tag)
	{
		bool isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	public override string ToString() => Name;

	protected DataSet(string name, ushort resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary();
		Weights = new WeightsLibrary();
	}

	protected void AddTag(TTag tag)
	{
		bool isNameAlreadyUsed = Tags.Any(existingTag => existingTag.Name == tag.Name);
		Guard.IsFalse(isNameAlreadyUsed);
		_tags.Add(tag);
	}

	private readonly List<TTag> _tags = new();
}