using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTag : Tag
{
	public override string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			foreach (var sibling in Library)
				Guard.IsNotEqualTo(sibling.Name, value);
			_name = value;
		}
	}

	public IReadOnlyList<KeyPointTag> KeyPoints => _keyPoints.AsReadOnly();
	public IReadOnlyCollection<PoserItem> Items => _items;
	public PoserTagsLibrary Library { get; }
	public PoserDataSet DataSet => Library.DataSet;

	public KeyPointTag AddKeyPoint(string name)
	{
		Guard.IsEmpty(Items);
		KeyPointTag tag = new(name, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag tag)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_keyPoints.Remove(tag));
	}

	internal PoserTag(string name, PoserTagsLibrary library)
	{
		_name = name;
		_keyPoints = new List<KeyPointTag>();
		_items = new HashSet<PoserItem>();
		Library = library;
	}

	internal void AddItem(PoserItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(PoserItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	private readonly List<KeyPointTag> _keyPoints;
	private readonly HashSet<PoserItem> _items;
	private string _name;

}