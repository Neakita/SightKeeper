using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTag : Tag
{
	public IReadOnlyList<Tag> KeyPoints => _keyPoints.AsReadOnly();

	public Tag AddKeyPoint(string name, uint color)
	{
		Tag tag = new(name, color);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(Tag tag)
	{
		bool isRemoved = _keyPoints.Remove(tag);
		Guard.IsTrue(isRemoved);
	}
	
	internal PoserTag(string name, uint color) : base(name, color)
	{
		_keyPoints = new List<Tag>();
	}

	private readonly List<Tag> _keyPoints;
}