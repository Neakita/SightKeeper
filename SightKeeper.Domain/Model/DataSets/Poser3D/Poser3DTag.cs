using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DTag : Tag
{
	public IReadOnlyList<KeyPointTag3D> KeyPoints => _keyPoints.AsReadOnly();
	public IReadOnlyCollection<Poser3DItem> Items => _items;
	public Poser3DTagsLibrary Library { get; }
	public Poser3DDataSet DataSet => Library.DataSet;

	public KeyPointTag3D CreateKeyPoint(string name)
	{
		Guard.IsEmpty(Items);
		KeyPointTag3D tag = new(name, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag3D tag)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_keyPoints.Remove(tag));
	}

	internal Poser3DTag(string name, Poser3DTagsLibrary library) : base(name, library)
	{
		Library = library;
		_keyPoints = new List<KeyPointTag3D>();
		_items = new HashSet<Poser3DItem>();
	}

	internal void AddItem(Poser3DItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(Poser3DItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	protected override IEnumerable<Tag> Siblings => Library;

	private readonly List<KeyPointTag3D> _keyPoints;
	private readonly HashSet<Poser3DItem> _items;
}