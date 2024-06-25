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
			bool isNewNameOccupied = Library.Any(tag => tag.Name == value);
			Guard.IsFalse(isNewNameOccupied);
			_name = value;
		}
	}

	public IReadOnlyList<KeyPointTag> KeyPoints => _keyPoints.AsReadOnly();
	public PoserTagsLibrary Library { get; }
	public PoserDataSet DataSet => Library.DataSet;

	internal PoserTag(string name, uint color, PoserTagsLibrary library)
	{
		_name = name;
		Color = color;
		_keyPoints = new List<KeyPointTag>();
		Library = library;
	}

	private string _name;

	public KeyPointTag AddKeyPoint(string name, uint color)
	{
		KeyPointTag tag = new(name, color, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag tag)
	{
		bool isRemoved = _keyPoints.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	private readonly List<KeyPointTag> _keyPoints;
}